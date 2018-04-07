using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{
    public class DtoBase<TKey>
    {
        protected DtoBase()
        {
        }
        
        public TKey Id { get; set; }
        
        public bool? IsValid { get; set; }

        [JsonConverter(typeof(CommonDateTimeConverter))]
        public DateTime? CreateTime { get; set; }
    }


    public class DicDto
    {
        public string uid { get; set; }

        public int id { get; set; }

        public string name { get; set; }
    }
}
