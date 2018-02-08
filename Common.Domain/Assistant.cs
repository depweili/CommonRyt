using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Assistant : EntityBase<int>
    {
        public string Name { get; set; }

        public string Level { get; set; }


    }

    //推送助手结构
    public class AssistantManager : EntityBase<int>
    {
        public int AssistantID { get; set; }
        public virtual Assistant Assistant { get; set; }

        public int? HospitalID { get; set; }
        public virtual Hospital Hospital { get; set; }

        //public int? MedicineCategoryID { get; set; }
        //public virtual MedicineCategory MedicineCategory { get; set; }
    }


    public class AssistantDoctor : EntityBase<int>
    {
        public int AssistantID { get; set; }
        public virtual Assistant Assistant { get; set; }

        public int DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

    }

    //患者推送
}
