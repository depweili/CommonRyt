using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Mappings
{
    public class UserIntegralMap : EntityTypeConfiguration<UserIntegral>
    {
        public UserIntegralMap()
        {
            this.HasRequired(t => t.User).WithRequiredDependent(t => t.UserIntegral);
            //.Map(c => c.MapKey("UserID"));只是改名
        }
    }

    public class IntegralGradeMap : EntityTypeConfiguration<IntegralGrade>
    {
        public IntegralGradeMap()
        {
            this.Property(t => t.Title).HasMaxLength(50);
            this.Property(t => t.Icon).HasMaxLength(50);
        }
    }

    public class IntegralActivityMap : EntityTypeConfiguration<IntegralActivity>
    {
        public IntegralActivityMap()
        {
            this.Property(t => t.Name).HasMaxLength(50);
            this.Property(t => t.Description).HasMaxLength(200);
            this.Property(t => t.Code).HasMaxLength(30);
        }
    }

    public class IntegralRuleMap : EntityTypeConfiguration<IntegralRule>
    {
        public IntegralRuleMap()
        {
            this.Property(t => t.Description).HasMaxLength(500);
            this.Property(t => t.CycleType).HasMaxLength(20);
            this.Property(t => t.StepInterval).HasMaxLength(20);
            this.Property(t => t.Formula).HasMaxLength(50);
        }
    }

    public class IntegralRecordMap : EntityTypeConfiguration<IntegralRecord>
    {
        public IntegralRecordMap()
        {
            this.Property(t => t.ShortMark).HasMaxLength(20);
            this.Property(t => t.Memo).HasMaxLength(200);
        }
    }

    public class IntegralSignInMap : EntityTypeConfiguration<IntegralSignIn>
    {
        public IntegralSignInMap()
        {
        }
    }

    public class IntegralUserActivityMap : EntityTypeConfiguration<IntegralUserActivity>
    {
        public IntegralUserActivityMap()
        {
            this.Property(t => t.Memo).HasMaxLength(50);
        }
    }
}
