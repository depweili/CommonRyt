﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    //调研
    public class Research : EntityBase<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }

    }

    public class ResearchSubject : EntityBase<int>
    {
        public int ResearchID { get; set; }
        public virtual Research Research{ get; set; }
        public string Subject { get; set; }

        public string Answer { get; set; }
    }

    public class ResearchSubjectOption : EntityBase<int>
    {
        public int ResearchSubjectID { get; set; }
        public virtual ResearchSubject ResearchSubject { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        
        public int Order { get; set; }
    }

    public class ResearchResult : EntityBase<int>
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public int ResearchSubjectID { get; set; }
        public virtual ResearchSubject ResearchSubject { get; set; }

        public int ResearchSubjectOptionID { get; set; }
        public virtual ResearchSubjectOption ResearchSubjectOption { get; set; }
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

    //文章
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
