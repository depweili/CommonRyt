using Common.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class CommonContext : DbContext
    {
        public CommonContext()
            : base("CommonContext")
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CommonContext>()); RytDbContextInitializer
            //Database.SetInitializer(new RytDbContextInitializer());
            Database.SetInitializer<CommonContext>(null);

            //this.Configuration.ProxyCreationEnabled = false;
            // 禁用延迟加载
            //this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string tbPrefix = ConfigHelper.GetConfig("tablePrefix");//ConfigurationManager.AppSettings["tablePrefix"];

            if (!string.IsNullOrEmpty(tbPrefix))
            {
                modelBuilder.Types().Configure(f => f.ToTable(tbPrefix + f.ClrType.Name));
            }

            var mappings = GetType().Assembly.GetInheritedTypes(typeof(EntityTypeConfiguration<>));
            foreach (var mapping in mappings)
            {
                dynamic instance = Activator.CreateInstance(mapping);
                modelBuilder.Configurations.Add(instance);
            }


            // 禁用默认表名复数形式
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // 禁用一对多级联删除
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // 禁用多对多级联删除
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}
