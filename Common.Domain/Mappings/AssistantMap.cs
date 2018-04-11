using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Mappings
{
    public class AssistantMap : EntityTypeConfiguration<Assistant>
    {
        public AssistantMap()
        {
            this.Property(t => t.Name).HasMaxLength(100);
            this.Property(t => t.AvatarUrl).HasMaxLength(200);
            this.Property(t => t.MobilePhone).HasMaxLength(50);
            this.Property(t => t.QQ).HasMaxLength(50);
            this.Property(t => t.WeChat).HasMaxLength(50);
            this.Property(t => t.InvitationCode).HasMaxLength(50);
        }
    }

    public class AssistantManagerMap : EntityTypeConfiguration<AssistantManager>
    {
        public AssistantManagerMap()
        {
        }
    }


    public class AssistantDoctorMap : EntityTypeConfiguration<AssistantDoctor>
    {
        public AssistantDoctorMap()
        {
        }
    }


}
