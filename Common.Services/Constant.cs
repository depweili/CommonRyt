using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    class Constant
    {
    }

    public enum YesOrNo
    {
        [Description("是")]
        Yes,
        [Description("否")]
        No
    }

    public enum AuditEnum
    {
        [Description("登记")]
        Holding = 0,
        [Description("审核中")]
        Auditing = 1,
        [Description("审核通过")]
        Pass = 2,
        [Description("驳回")]
        Reject = 3
    }
}
