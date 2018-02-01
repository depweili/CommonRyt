using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class User : EntityBase<int>
    {
        public User()
        {
            IsValid = true;
            CreateTime = DateTime.Now;
            AuthID = Guid.NewGuid();
            UserName = RandomHelper.GetRandomString(8);
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Guid AuthID { get; set; }

        public DateTime? LastActiveTime { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        //public virtual Agent Agent { get; set; }

        public virtual UserIntegral UserIntegral { get; set; }
    }

    public class UserProfile : EntityBase<int>
    {
        public UserProfile()
        {
            IsVerified = false;
        }
        public int ID { get; set; }

        public string NickName { get; set; }

        public string AvatarUrl { get; set; }

        public string RealName { get; set; }

        public int? Gender { get; set; }

        public int? Age { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string MobilePhone { get; set; }

        public string IDCard { get; set; }

        public bool? IsVerified { get; set; }

        public virtual User User { get; set; }
    }

    public class UserAuth : EntityBase<int>
    {
        public UserAuth()
        {
        }

        public string IdentityType { get; set; }

        public string Identifier { get; set; }

        public string Credential { get; set; }

        public DateTime? LastActiveTime { get; set; }

        public virtual User User { get; set; }
    }
}
