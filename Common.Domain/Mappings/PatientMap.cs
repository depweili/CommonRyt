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
            this.Property(t => t.Content).HasMaxLength(50);
        }
    }

    public class ReadPatientRecordMap : EntityTypeConfiguration<ReadPatientRecord>
    {
        public ReadPatientRecordMap()
        {
            this.Property(t => t.Diagnostic).HasMaxLength(50);
        }
    }

}
