using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Mappings
{
    public class PatientMap : EntityTypeConfiguration<Patient>
    {
        public PatientMap()
        {
            this.Property(t => t.Name).HasMaxLength(50);
            this.Property(t => t.Address).HasMaxLength(50);
            this.Property(t => t.Telephone).HasMaxLength(30);
            this.Property(t => t.AreaInfo).HasMaxLength(50);
        }
    }

    public class PatientDoctorMap : EntityTypeConfiguration<PatientDoctor>
    {
        public PatientDoctorMap()
        {
        }
    }

    public class PatientMedicalRecordMap : EntityTypeConfiguration<PatientMedicalRecord>
    {
        public PatientMedicalRecordMap()
        {
            this.Property(t => t.Content).HasColumnType("ntext");
        }
    }

    public class ReadPatientRecordMap : EntityTypeConfiguration<ReadPatientRecord>
    {
        public ReadPatientRecordMap()
        {
            this.Property(t => t.Diagnostic).HasMaxLength(1000);
        }
    }

    public class CharityDrugApplicationMap : EntityTypeConfiguration<CharityDrugApplication>
    {
        public CharityDrugApplicationMap()
        {
            this.Property(t => t.Address).HasMaxLength(100);
            this.Property(t => t.Area).HasMaxLength(100);
            this.Property(t => t.Chemotherapy).HasMaxLength(100);
            this.Property(t => t.DrugName).HasMaxLength(100);
            this.Property(t => t.EmergencyContact).HasMaxLength(100);
            this.Property(t => t.EmergencyPhone).HasMaxLength(100);
            this.Property(t => t.IDCard).HasMaxLength(100);
            this.Property(t => t.MedicalRecordNo).HasMaxLength(100);
            this.Property(t => t.PatientName).HasMaxLength(100);
            this.Property(t => t.MobilePhone).HasMaxLength(100);
        }
    }

}
