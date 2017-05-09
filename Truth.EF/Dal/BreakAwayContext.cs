using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Truth.EF.Model;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

namespace Truth.EF.Dal
{
    public class BreakAwayContext:DbContext
    {
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Lodging> Lodgings { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Person> Pepole { get; set; }
        public DbSet<PersonPhoto> PersonPhotos { get; set; }
        public DbSet<InternetSpecial> InternetSpecials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
           // 移除这个默认约定，再在需要开启级联删除的FluentAPI关系映射中用.WillCascadeOnDelete(true) 单独开启
            //http://www.cnblogs.com/oppoic/p/ef_default_mapping_and_data_annotations_fluent_api.html
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();//移除复数表名的契约
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();//防止黑幕交易 要不然每次都要访问 
            // modelBuilder.Entity<Destination>().Property(d => d.Name).IsRequired();
            modelBuilder.Configurations.Add(new DestinationMap());
            modelBuilder.Configurations.Add(new LodgingMap());
            modelBuilder.Configurations.Add(new TripMap());
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new PersonPhotoMap());
            modelBuilder.ComplexType<Address>();
            modelBuilder.Configurations.Add(new InternetSpecialMap());
        }
    }
}
