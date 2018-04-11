using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class ReadPatientRecord : EntityBase<int>
    {
        public int PatientMedicalRecordID { get; set; }
        public virtual PatientMedicalRecord PatientMedicalRecord { get; set; }

        public int DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        public string Diagnostic { get; set; }

        public string Memo { get; set; }
    }
}
