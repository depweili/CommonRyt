using AutoMapper;
using Common.Domain;
using Common.Services.Dtos;
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
                cfg.CreateMap<Hospital, HospitalDto>();
                cfg.CreateMap<MedicineCategory, MedicineCategoryDto>();
                cfg.CreateMap<Doctor, DoctorDto>().ForMember(t => t.HospitalName, opt => opt.MapFrom(src => src.MedicineDepartment.Hospital.Name))
                .ForMember(t => t.MedicineCategoryName, opt => opt.MapFrom(src => src.MedicineDepartment.MedicineCategory.Name));

                cfg.CreateMap<Patient, PatientDto>();

                cfg.CreateMap<CharityDrugApplication, CharityDrugApplicationDto>()
                .ForMember(t => t.StrCharityDrugUid, opt => opt.MapFrom(src=>src.CharityDrugUid.ToString()));

                cfg.CreateMap<CharityDrugApplicationDto, CharityDrugApplication>()
                .ForMember(t => t.CreateTime, opt => opt.Ignore())
                .ForMember(t => t.Id, opt => opt.Ignore());


                cfg.CreateMap<PatientMedicalRecord, PatientMedicalRecordDto>();

                cfg.CreateMap<PatientMedicalRecordDto, PatientMedicalRecord>()
                .ForMember(t => t.CreateTime, opt => opt.Ignore())
                .ForMember(t => t.Id, opt => opt.Ignore());

                //cfg.AddProfile<Profiles.SourceProfile>();
                //cfg.AddProfile<Profiles.OrderProfile>();
                //cfg.AddProfile<Profiles.CalendarEventProfile>();


            });

            //Mapper.AssertConfigurationIsValid();
        }
    }
}
