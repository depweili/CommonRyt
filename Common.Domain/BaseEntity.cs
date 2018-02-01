using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class EntityBase<TKey>
    {
        protected EntityBase()
        {
            IsDeleted = false;
            IsValid = true;
            CreateTime= DateTime.Now;
        }

        [Key]
        public TKey Id { get; set; }

        public DateTime CreateTime { get; set; }

        public bool? IsValid { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
