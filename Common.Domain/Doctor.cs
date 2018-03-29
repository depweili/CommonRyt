using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Hospital : EntityBase<int>
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public int Level { get; set; }

        public string Introduction { get; set; }

        public string Address { get; set; }


        public int? AreaID { get; set; }
        public BaseArea Area { get; set; }

        public virtual ICollection<MedicineDepartment> MedicineDepartments { get; set; }
    }

    public class MedicineCategory : EntityBase<int>
    {
        public string Name { get; set; }

        public int Order { get; set; }
    }

    public class MedicineDepartment : EntityBase<int>
    {
        public int MedicineCategoryID { get; set; }
        public virtual MedicineCategory MedicineCategory { get; set; }

        public int HospitalID { get; set; }
        public virtual Hospital Hospital { get; set; }


        public virtual ICollection<Doctor> Doctors { get; set; }
    }

    public class Doctor : EntityBase<int>
    {

        public Doctor()
        {
            IsVerified = false;
            Uid = Guid.NewGuid();
            Code = RandomHelper.GetRandomString(8);
        }

        public Guid Uid { get; set; }

        public virtual User User { get; set; }

        public int? MedicineDepartmentID { get; set; }
        public virtual MedicineDepartment MedicineDepartment { get; set; }

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

    }

    public class MedicalRecord : SubjectEntity<int>
    {
        public MedicalRecord()
        {
            MedicalRecordUid = Guid.NewGuid();
        }
        public Guid MedicalRecordUid { get; set; }

        public int? DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        public int MedicineCategoryID { get; set; }
        public virtual MedicineCategory MedicineCategory { get; set; }

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

        public int? ArticleID { get; set; }
        public virtual Article Article { get; set; }
        
        public decimal? Order { get; set; }

    }

    //未启用 图片直接guid关联
    public class MedicalRecordImage : EntityBase<int>
    {
        public int MedicalRecordID { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }

        public int ImageInfoID { get; set; }
        public virtual ImageInfo ImageInfo { get; set; }

        public string Memo { get; set; }

        public int Order { get; set; }

    }


}
