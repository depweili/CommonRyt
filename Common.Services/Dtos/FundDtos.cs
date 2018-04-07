using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{
    public class FundDto : DtoBase<int>
    {
        public FundDto()
        {
            FundProjects = new List<FundProjectDto>();
        }
        public int ID { get; set; }
        public Guid FundUid { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }

        public string Introduction { get; set; }

        public decimal Order { get; set; }

        public List<FundProjectDto> FundProjects { get; set; }

        public string ArticleUID { get; set; }
    }


    public class FundProjectDto : DtoBase<int>
    {
        public Guid FundProjectUid { get; set; }

        public string FrontPic { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }

        public decimal? Order { get; set; }

        [JsonConverter(typeof(CommonDateConverter))]
        public new DateTime? CreateTime { get; set; }

        public string ArticleUID { get; set; }
    }
}
