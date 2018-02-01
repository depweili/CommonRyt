using Common.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.Property(t => t.UserName).HasMaxLength(50);
            this.Property(t => t.Password).HasMaxLength(50);
        }
    }

    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            this.Property(t => t.NickName).HasMaxLength(50);
            this.Property(t => t.RealName).HasMaxLength(50);
            this.Property(t => t.IDCard).HasMaxLength(50);
            this.Property(t => t.Email).HasMaxLength(50);
            this.Property(t => t.Address).HasMaxLength(50);
            this.Property(t => t.MobilePhone).HasMaxLength(50);
            this.Property(t => t.AvatarUrl).HasMaxLength(200);

            this.HasRequired(t => t.User)
                .WithRequiredDependent(t => t.UserProfile);
        }
    }

    public class UserAuthMap : EntityTypeConfiguration<UserAuth>
    {
        public UserAuthMap()
        {
            this.Property(t => t.IdentityType).HasMaxLength(50).IsUnique("UserAuthIdentity", 1);
            this.Property(t => t.Identifier).HasMaxLength(50).IsUnique("UserAuthIdentity", 2);
            this.Property(t => t.Credential).HasMaxLength(100);
        }
    }
}
