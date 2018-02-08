﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Mappings
{

    public class ArticleMap : EntityTypeConfiguration<Article>
    {
        public ArticleMap()
        {
            this.Property(t => t.Title).HasMaxLength(200);
            this.Property(t => t.Content).HasColumnType("ntext");
            this.Property(t => t.Code).HasMaxLength(20);
            this.Property(t => t.Author).HasMaxLength(20);
            //this.Property(t => t.CreateTime).IsRequired();

            //this.Map(t => t.Requires("CreateTime").HasValue<DateTime>(DateTime.Now));
        }
    }

    public class NavigationMap : EntityTypeConfiguration<Navigation>
    {
        public NavigationMap()
        {
            this.Property(t => t.Desc).IsRequired().HasMaxLength(50);
            this.Property(t => t.Pic).HasMaxLength(50);
            this.Property(t => t.Target).HasMaxLength(100);
        }
    }
}