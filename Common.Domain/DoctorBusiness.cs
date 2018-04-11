using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    //调研
    public class Survey : EntityBase<int>
    {
        public Survey()
        {
            SurveyUid = Guid.NewGuid();
        }
        public Guid SurveyUid { get; set; }

        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string FrontPic { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Count { get; set; }

        public int? Order { get; set; }

        public string Memo { get; set; }
    }

    public class SurveyAuth : EntityBase<int>
    {
        public int SurveyID { get; set; }
        public virtual Survey Survey { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }
    }

    public class SurveyQuestion : EntityBase<int>
    {
        public int SurveyID { get; set; }
        public virtual Survey Survey { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        public int Type { get; set; }

        public virtual ICollection<SurveyQuestionOption> items { get; set; }


    }

    public class SurveyQuestionOption : EntityBase<int>
    {
        public int SurveyQuestionID { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }

        public string Value { get; set; }
        public string Content { get; set; }

        public int SelectCount { get; set; }

        public int? Order { get; set; }
    }

    public class SurveyAnswer : EntityBase<int>
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int SurveyQuestionID { get; set; }
        public virtual SurveyQuestion SurveyQuestion { get; set; }

        //public int SurveyQuestionOptionID { get; set; }
        //public virtual SurveyQuestionOption SurveyQuestionOption { get; set; }
        //public bool IsSelect { get; set; }

        public string Answer { get; set; }

        public string Memo { get; set; }
    }

    public class SurveyUser : EntityBase<int>
    {
        public int SurveyID { get; set; }
        public virtual Survey Survey { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int State { get; set; }

        public string Memo { get; set; }
    }


    public class Conference : SubjectEntity<int>
    {
        public Conference()
        {
            ConferenceUid = Guid.NewGuid();
        }
        public Guid ConferenceUid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Memo { get; set; }

        public int State { get; set; }

        public int? ArticleID { get; set; }

        public virtual Article Article { get; set; }
    }

    public class MyConference : EntityBase<int>
    {
        public int ConferenceID { get; set; }
        public virtual Conference Conference { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }
    }

    public class Attention : EntityBase<int>
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public string Type { get; set; }

        public Guid Uid { get; set; }

        public string ArticleUid  { get; set; }

        public string Title { get; set; }

        public string PicUrl { get; set; }
    }


    //医学资料
    public class MedicalToolData : EntityBase<int>
    {
        public MedicalToolData()
        {
            MedicalToolUid = Guid.NewGuid();
        }
        public Guid MedicalToolUid { get; set; }

        public int Type { get; set; }

        public string Title { get; set; }

        public string FrontPic { get; set; }

        public string Content { get; set; }

        public int? ArticleID { get; set; }

        public virtual Article Article { get; set; }

        public string Memo { get; set; }

        public int Order { get; set; }

    }

    //文章 没用
    public class Literature : EntityBase<int>
    {
        public string Title { get; set; }
        public string Introduction { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }
    }

    //


}
