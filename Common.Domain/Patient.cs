using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Patient : EntityBase<int>
    {
        public Patient()
        {
            Uid = Guid.NewGuid();
        }
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }

        public DateTime Birthday { get; set; }
        public string Telephone { get; set; }
        public string AreaInfo { get; set; }
        public string Address { get; set; }

        public int Height { get; set; }
        public int Weight { get; set; }

        public string Health { get; set; }

        public int? UserID { get; set; }
        public virtual User User { get; set; }
    }

    public class PatientDoctor : EntityBase<int>
    {
        public int? PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        public int? DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        //消息提醒
        public int NewRecord { get; set; }
        public bool IsRead { get; set; }

        public int? PatientOrder { get; set; }
        public int? DoctorOrder { get; set; }
        
        public int? State { get; set; }
    }

    public class PatientMedicalRecord : EntityBase<int>
    {
        public int? PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        public string Content { get; set; }
    }

    

    public class CharityDrugApplication : EntityBase<int>
    {
        public int? PatientID { get; set; }
        public virtual Patient Patient { get; set; }


    }
    
}
