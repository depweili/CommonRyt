using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class UserIntegral : EntityBase<int>
    {
        public UserIntegral()
        {
            IntegralID = Guid.NewGuid();
        }

        public Guid IntegralID { get; set; }

        public int TotalPoints { get; set; }

        public int CurrentPoints { get; set; }

        public int TotalExpense { get; set; }

        public int? IntegralGradeID { get; set; }
        public virtual IntegralGrade IntegralGrade { get; set; }

        //[ForeignKey("User")]
        //public int UserID { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<IntegralUserActivity> IntegralUserActivitys { get; set; }

        public virtual ICollection<IntegralRecord> IntegralRecords { get; set; }

        //public virtual IntegralSignIn IntegralSignIn { get; set; }

    }

    public class IntegralGrade : EntityBase<int>
    {
        public IntegralGrade()
        {
        }

        public int Grade { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }
    }


    public class IntegralActivity : EntityBase<int>
    {
        public IntegralActivity()
        {
        }

        public int Type { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? MinGrade { get; set; }

        public int? MaxGrade { get; set; }

        public int? ArticleID { get; set; }
        public virtual Article Article { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? IsOpen { get; set; }

        public int? IntegralRuleID { get; set; }
        public virtual IntegralRule IntegralRule { get; set; }
    }

    public class IntegralRule : EntityBase<int>
    {
        public IntegralRule()
        {
        }

        public int Type { get; set; }

        public string Description { get; set; }

        public int? ArticleID { get; set; }

        public virtual Article Article { get; set; }

        public int? Points { get; set; }

        public string Formula { get; set; }

        public int? StepPoints { get; set; }

        public string StepInterval { get; set; }

        //public decimal PeriodMaxPoints { get; set; }

        public string CycleType { get; set; }

        public int? MaxPoints { get; set; }

        public int? MaxTotalPoints { get; set; }
    }

    public class IntegralRecord : EntityBase<int>
    {
        public IntegralRecord()
        {
        }

        public string ShortMark { get; set; }

        public int Points { get; set; }

        public int TotalPoints { get; set; }

        public int CurrentPoints { get; set; }

        public int ValidPoints { get; set; }

        public int LockPoints { get; set; }

        public int ExpensePoints { get; set; }

        public int ExpiredPoints { get; set; }

        public string Memo { get; set; }

        public Nullable<DateTime> ExpiredTime { get; set; }

        public DateTime RecordTime { get; set; }

        public int? IntegralActivityID { get; set; }
        public virtual IntegralActivity IntegralActivity { get; set; }

        public int? UserIntegralID { get; set; }
        public virtual UserIntegral UserIntegral { get; set; }
    }

    public class IntegralSignIn : EntityBase<int>
    {

        public DateTime? LastTime { get; set; }

        public int DurationDays { get; set; }

        [ForeignKey("UserIntegral")]
        public int? UserIntegralID { get; set; }
        public virtual UserIntegral UserIntegral { get; set; }
    }

    public class IntegralUserActivity : EntityBase<int>
    {
        public IntegralUserActivity()
        {
        }

        public string Memo { get; set; }

        public int TotalPoints { get; set; }

        public int UserIntegralID { get; set; }
        public virtual UserIntegral UserIntegral { get; set; }

        public int IntegralActivityID { get; set; }
        public virtual IntegralActivity IntegralActivity { get; set; }

        public int State { get; set; }

        public DateTime? CompleteTime { get; set; }
    }
}
