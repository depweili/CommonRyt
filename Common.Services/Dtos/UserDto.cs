using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{
    public class UserDto
    {
        //public int id { get; set; }

        public Guid authid { get; set; }

        public string realname { get; set; }

        public int? gender { get; set; }

        public string mobilephone { get; set; }

        public string idcard { get; set; }

        public string address { get; set; }

        public string agentcode { get; set; }

        public bool isagent { get; set; }

        public bool issignin { get; set; }

        public bool? isvalid { get; set; }

        public bool? isverified { get; set; }

        public Guid patientuid { get; set; }
    }


    public class UserProfileDto
    {
        public Guid authid { get; set; }

        public string nickname { get; set; }

        public string realname { get; set; }

        public int? gender { get; set; }

        public DateTime? birthday { get; set; }

        public string mobilephone { get; set; }

        public string idcard { get; set; }

        public string area { get; set; }

        public string address { get; set; }

        public bool isverified { get; set; }

    }
}
