using AutoMapper;
using Common.Domain;
using Common.Services.Dtos;
using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.AutoMapper
{
    public class Configuration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                //cfg.AddProfile<Profiles.SourceProfile>();
                //cfg.AddProfile<Profiles.OrderProfile>();
                //cfg.AddProfile<Profiles.CalendarEventProfile>();

                cfg.CreateMap<Hospital, HospitalDto>();
                cfg.CreateMap<MedicineCategory, MedicineCategoryDto>();
                cfg.CreateMap<Doctor, DoctorDto>()
                .ForMember(t => t.HospitalName, opt => opt.MapFrom(src => src.MedicineDepartment.Hospital.Name))
                .ForMember(t => t.MedicineCategoryName, opt => opt.MapFrom(src => src.MedicineDepartment.MedicineCategory.Name));

                cfg.CreateMap<Patient, PatientDto>();

                cfg.CreateMap<CharityDrugApplication, CharityDrugApplicationDto>()
                .ForMember(t => t.StrCharityDrugUid, opt => opt.MapFrom(src=>src.CharityDrugUid.ToString()))
                .ForMember(t => t.StateName, opt => opt.MapFrom(src => Function.GetDicDesc("审批",src.State)))
                .ForMember(t => t.ProjectDoctorName, opt => opt.MapFrom(src => src.ProjectDoctor.Name));

                cfg.CreateMap<CharityDrugApplicationDto, CharityDrugApplication>()
                .ForMember(t => t.CreateTime, opt => opt.Ignore())
                .ForMember(t => t.Id, opt => opt.Ignore());


                cfg.CreateMap<PatientMedicalRecord, PatientMedicalRecordDto>();

                cfg.CreateMap<PatientMedicalRecordDto, PatientMedicalRecord>()
                .ForMember(t => t.CreateTime, opt => opt.Ignore())
                .ForMember(t => t.Id, opt => opt.Ignore());


                cfg.CreateMap<Navigation, NavigationDto>()
                .ForMember(t => t.ArticleUID, opt => opt.MapFrom(src => src.Article == null ? "" : src.Article.ArticleUID.ToString()))
                .ForMember(t => t.PicUrl, opt => opt.MapFrom(src => Function.GetStaticPicUrl(src.Pic, null)));

                cfg.CreateMap<Article, ArticleDto>()
                .ForMember(t => t.Content, opt => opt.MapFrom(src => src.Content.Replace("{host}", Function.GetHostAndApp())))
                .ForMember(t => t.FrontPic, opt => opt.MapFrom(src => Function.GetStaticPicUrl(src.FrontPic,null)));


                cfg.CreateMap<Comment, CommentListDto>()
                .ForMember(t => t.UserName, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.User.UserProfile.NickName) ? StringHelper.ReplaceWithSpecialChar(src.User.UserName, 3, 4, '*') : src.User.UserProfile.NickName))
                .ForMember(t => t.AuthID, opt => opt.MapFrom(src => src.User.AuthID))
                .ForMember(t => t.AvatarUrl, opt => opt.MapFrom(src => src.User.UserProfile.AvatarUrl))
                .ForMember(t => t.CreateTime, opt => opt.MapFrom(src => src.CreateTime.ToAgoDateFomat()));


                cfg.CreateMap<MedicalRecordDto, MedicalRecord>()
                .ForMember(t => t.CreateTime, opt => opt.Ignore())
                .ForMember(t => t.Id, opt => opt.Ignore());

                cfg.CreateMap<MedicalRecord, MedicalRecordDto>()
                .ForMember(t => t.StrMedicalRecordUid, opt => opt.MapFrom(src => src.MedicalRecordUid.ToString()));

                cfg.CreateMap<Conference, ConferenceDto>();

                cfg.CreateMap<VideoInfo, VideoInfoDto>();


                cfg.CreateMap<Survey, SurveyDto>();

                cfg.CreateMap<SurveyQuestion, SurveyQuestionDto>();

                cfg.CreateMap<SurveyQuestionOption, SurveyQuestionOptionDto>();

                

            });

            //Mapper.AssertConfigurationIsValid();
        }
    }
}
