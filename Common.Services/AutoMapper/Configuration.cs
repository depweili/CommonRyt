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
                cfg.CreateMap<Doctor, DoctorDto>();

                cfg.CreateMap<Patient, PatientDto>();

                //cfg.AddProfile<Profiles.SourceProfile>();
                //cfg.AddProfile<Profiles.OrderProfile>();
                //cfg.AddProfile<Profiles.CalendarEventProfile>();
            });
        }
    }
}
