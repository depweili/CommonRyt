using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{
    public class SurveyDto
    {
        public Guid SurveyUid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string FrontPic { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Count { get; set; }

        public bool? IsValid { get; set; }

        public bool IsFinish { get; set; }

        public bool IsAnswer { get; set; }
    }

    public class SurveyQuestionDto
    {
        public int id { get; set; }

        public int number { get; set; }

        public string title { get; set; }

        public int type { get; set; }

        public string chooseitem { get; set; }

        public List<SurveyQuestionOptionDto> items { get; set; }


    }

    public class SurveyQuestionOptionDto
    {
        public string value { get; set; }
        public string content { get; set; }
        public bool chosen { get; set; }

        public int selectcount { get; set; }

        [JsonConverter(typeof(DecimalDigitsConverter))]
        public decimal percentage { get; set; }
    }
}
