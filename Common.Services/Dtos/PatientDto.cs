﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{
    public class PatientDto : DtoBase<int>
    {
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }

        public DateTime? Birthday { get; set; }
        public string Telephone { get; set; }
        public string AreaInfo { get; set; }
        public string Address { get; set; }

        public int Height { get; set; }
        public int Weight { get; set; }

        public string Health { get; set; }

        public string AvatarUrl { get; set; }

        public string MedicalRecord { get; set; }

        public string MedicalRecordUid { get; set; }
    }

    public class CharityDrugApplicationDto : DtoBase<int>
    {
        public Guid PatientUid { get; set; }

        public string StrCharityDrugUid { get; set; }

        public string PatientName { get; set; }

        public int? Gender { get; set; }

        public DateTime? BirthDay { get; set; }
        public string MobilePhone { get; set; }

        public string IDCard { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }

        public string EmergencyContact { get; set; }

        public string EmergencyPhone { get; set; }

        public string MedicalRecordNo { get; set; }

        public string Chemotherapy { get; set; }

        public string DrugName { get; set; }

        public DateTime? BeginDrugTime { get; set; }

        public int? ProjectDoctorID { get; set; }

        public string ProjectDoctorName { get; set; }

        public int State { get; set; }

        public string StateName { get; set; }
    }
    

    public class PatientMedicalRecordDto : DtoBase<int>
    {
        public Guid PatientUid { get; set; }
        public string Content { get; set; }
    }

}
