using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Mappings
{
    public class HospitalMap : EntityTypeConfiguration<Hospital>
    {
        public HospitalMap()
        {
            this.Property(t => t.Name).HasMaxLength(50);
            this.Property(t => t.FullName).HasMaxLength(50);
            this.Property(t => t.Introduction).HasMaxLength(200);
            this.Property(t => t.Address).HasMaxLength(100);
        }
    }
    public class MedicineCategoryMap : EntityTypeConfiguration<MedicineCategory>
    {
        public MedicineCategoryMap()
        {
            this.Property(t => t.Name).HasMaxLength(50);
        }
    }
    public class MedicineDepartmentMap : EntityTypeConfiguration<MedicineDepartment>
    {
        public MedicineDepartmentMap()
        {
        }
    }

    public class DoctorMap : EntityTypeConfiguration<Doctor>
    {
        public DoctorMap()
        {
            this.Property(t => t.Avatar).HasMaxLength(200);
            this.Property(t => t.Certificate).HasMaxLength(50);
            this.Property(t => t.Academic).HasMaxLength(200);
            this.Property(t => t.CertificatePhoto).HasColumnType("image");
            this.Property(t => t.Expert).HasMaxLength(200);
            this.Property(t => t.DepartmentAlias).HasMaxLength(100);
            this.Property(t => t.Introduction).HasMaxLength(500);
            this.Property(t => t.Level).HasMaxLength(20);
            this.Property(t => t.Name).HasMaxLength(20);
            this.Property(t => t.Code).HasMaxLength(20);
            this.Property(t => t.OpenMessage).HasMaxLength(20);
            this.Property(t => t.OutpatientSchedule).HasMaxLength(20);
            this.Property(t => t.Tag).HasMaxLength(20);
            this.Property(t => t.Title).HasMaxLength(20);
            this.Property(t => t.EduTitle).HasMaxLength(20);
        }
    }


    public class ConferenceMap : EntityTypeConfiguration<Conference>
    {
        public ConferenceMap()
        {
            this.Property(t => t.Address).HasMaxLength(200);
            this.Property(t => t.Content).HasMaxLength(500);
            this.Property(t => t.Country).HasMaxLength(200);
            this.Property(t => t.Title).HasMaxLength(100);
            this.Property(t => t.Memo).HasMaxLength(200);
            this.Property(t => t.FrontPic).HasMaxLength(200);
            this.Property(t => t.City).HasMaxLength(200);
        }
    }

    public class AttentionMap : EntityTypeConfiguration<Attention>
    {
        public AttentionMap()
        {
            this.Property(t => t.PicUrl).HasMaxLength(200);
            this.Property(t => t.Title).HasMaxLength(100);
            this.Property(t => t.Type).HasMaxLength(100);
            this.Property(t => t.ArticleUid).HasMaxLength(50); 
        }
    }


    public class MedicalRecordMap : EntityTypeConfiguration<MedicalRecord>
    {
        public MedicalRecordMap()
        {
            this.Property(t => t.Content).HasMaxLength(500);
            this.Property(t => t.Title).HasMaxLength(200);
            this.Property(t => t.FrontPic).HasMaxLength(200);
            this.Property(t => t.Diagnosis).HasMaxLength(500);
            this.Property(t => t.MedicalHistory).HasMaxLength(500);
            this.Property(t => t.Inspection).HasMaxLength(500);
            this.Property(t => t.PhysicalExamination).HasMaxLength(500);
        }
    }

    public class FundMap : EntityTypeConfiguration<Fund>
    {
        public FundMap()
        {
            this.Property(t => t.Name).HasMaxLength(200);
            this.Property(t => t.Introduction).HasMaxLength(500);
        }
    }

    public class FundProjectMap : EntityTypeConfiguration<FundProject>
    {
        public FundProjectMap()
        {
            this.Property(t => t.Name).HasMaxLength(200);
            this.Property(t => t.Introduction).HasMaxLength(500);
        }
    }

}
