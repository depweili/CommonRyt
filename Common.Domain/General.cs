using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{

    public class BaseArea
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? Pid { get; set; }

        public int? Level { get; set; }

        public int? Type { get; set; }

        public string Name { get; set; }

        public string Area { get; set; }
    }


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

    public class FeedBack : EntityBase<int>
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }

        public string Content { get; set; }

        public string Process { get; set; }

        public int State { get; set; }
    }

}
