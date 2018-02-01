using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class Article : EntityBase<int>
    {
        public Article()
        {
            ArticleUID = Guid.NewGuid();
        }

        public Guid ArticleUID { get; set; }

        public string Code { get; set; }

        public int Type { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

    }

    public class Navigation : EntityBase<int>
    {
        public Navigation()
        {
        }

        public int Type { get; set; }

        public string Desc { get; set; }

        public string Pic { get; set; }

        public string Target { get; set; }

        public int Order { get; set; }

        public int? ArticleID { get; set; }

        public virtual Article Article { get; set; }

    }
}
