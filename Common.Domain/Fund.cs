using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Fund : EntityBase<int>
    {
        public Fund()
        {
            FundUid = Guid.NewGuid();
        }
        public Guid FundUid { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }

        public string Introduction { get; set; }

        public decimal Order { get; set; }
    }

    public class FundProject : EntityBase<int>
    {
        public FundProject()
        {
            FundProjectUid = Guid.NewGuid();
        }
        public Guid FundProjectUid { get; set; }

        public int FundID { get; set; }
        public virtual Fund Fund { get; set; }
        
        public string Name { get; set; }
        public string Introduction { get; set; }

        public decimal? Order { get; set; }

    }


    public class FundMedicalRecord : EntityBase<int>
    {
        public FundMedicalRecord()
        {
            
        }
        public int FundProjectID { get; set; }
        public virtual FundProject FundProject { get; set; }

        public int MedicalRecordID { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; }

        public int State { get; set; }

        public decimal? Order { get; set; }

    }
    


}
