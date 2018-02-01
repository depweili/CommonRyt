﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Dtos
{
    public class ArticleDto
    {
        public Guid ArticleUID { get; set; }

        public string Code { get; set; }

        public int Type { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }

    public class NavigationDto
    {
        public int Type { get; set; }

        public string Desc { get; set; }

        public string Pic { get; set; }

        public string Target { get; set; }

        public int Order { get; set; }

        public int? ArticleID { get; set; }
    }
}
