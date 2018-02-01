using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class ResponseBase
    {
        public string code { get; set; }
        public string msg { get; set; }
        public dynamic resData { get; set; }

        public ResponseBase()
        {
            code = "0";
            msg = "";
        }
    }
}
