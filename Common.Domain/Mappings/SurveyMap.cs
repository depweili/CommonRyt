using Common.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Mappings
{

    public class SurveyMap : EntityTypeConfiguration<Survey>
    {
        public SurveyMap()
        {
            this.Property(t => t.Title).HasMaxLength(100);
            this.Property(t => t.Description).HasMaxLength(200);
            this.Property(t => t.FrontPic).HasMaxLength(100);
            this.Property(t => t.Memo).HasMaxLength(100);
        }
    }

    public class SurveyQuestionMap : EntityTypeConfiguration<SurveyQuestion>
    {
        public SurveyQuestionMap()
        {
            this.Property(t => t.Title).HasMaxLength(200);
        }
    }

    public class SurveyQuestionOptionMap : EntityTypeConfiguration<SurveyQuestionOption>
    {
        public SurveyQuestionOptionMap()
        {
            this.Property(t => t.Content).HasMaxLength(50);
            this.Property(t => t.Value).HasMaxLength(5);
        }
    }

    public class SurveyAnswerMap : EntityTypeConfiguration<SurveyAnswer>
    {
        public SurveyAnswerMap()
        {
            this.Property(t => t.Answer).HasMaxLength(200);
            this.Property(t => t.Memo).HasMaxLength(100);
        }
    }

    public class SurveyUserMap : EntityTypeConfiguration<SurveyUser>
    {
        public SurveyUserMap()
        {
            this.Property(t => t.Memo).HasMaxLength(100);
            this.Property(t => t.SurveyID).IsUnique("UKSurveyUser", 1);
            this.Property(t => t.UserID).IsUnique("UKSurveyUser", 2);
        }
    }

    public class SurveyAuthMap : EntityTypeConfiguration<SurveyAuth>
    {
        public SurveyAuthMap()
        {
            this.Property(t => t.SurveyID).IsUnique("UKSurveyAuth", 1);
            this.Property(t => t.UserID).IsUnique("UKSurveyAuth", 2);
        }
    }
}
