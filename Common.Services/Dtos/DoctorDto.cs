﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{
    public class HospitalDto: DtoBase<int>
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public int Level { get; set; }

        public string Introduction { get; set; }

        public string Address { get; set; }

        //public BaseArea Area { get; set; }

        //public virtual ICollection<MedicineDepartment> MedicineDepartments { get; set; }
    }

    public class MedicineCategoryDto : DtoBase<int>
    {
        public string Name { get; set; }

        public int Order { get; set; }
    }


    public class MedicalRecordDto : DtoBase<int>
    {
        public string StrMedicalRecordUid { get; set; }

        public int MedicineCategoryID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        //病史
        public string MedicalHistory { get; set; }
        //查体
        public string PhysicalExamination { get; set; }

        //检查
        public string Inspection { get; set; }
        //诊断
        public string Diagnosis { get; set; }
    }


    public class DoctorDto : DtoBase<int>
    {

        public Guid Uid { get; set; }
        
        public string HospitalName { get; set; }

        public string MedicineCategoryName { get; set; }

        //细微别名
        public string DepartmentAlias { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Avatar { get; set; }

        public int? Gender { get; set; }

        public string Certificate { get; set; }

        public byte[] CertificatePhoto { get; set; }

        public string Title { get; set; }

        public string EduTitle { get; set; }

        public string Level { get; set; }

        public string Tag { get; set; }

        public string Expert { get; set; }

        public string Academic { get; set; }

        public string Introduction { get; set; }

        public string OpenMessage { get; set; }

        public string OutpatientSchedule { get; set; }

        public bool IsVerified { get; set; }

        public decimal Score { get; set; }

        public bool IsConnect { get; set; }

    }
}
