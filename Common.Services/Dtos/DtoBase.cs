using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{
    public class DtoBase<TKey>
    {
        protected DtoBase()
        {
        }
        
        public TKey Id { get; set; }
        
        public bool? IsValid { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
