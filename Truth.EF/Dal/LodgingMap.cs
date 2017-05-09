using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Truth.EF.Model;

namespace Truth.EF.Dal
{
    public class LodgingMap:EntityTypeConfiguration<Lodging>
    {
        public LodgingMap()
        {
            //HasRequired(d => d.Destination).WithMany(d => d.Lodgings).HasForeignKey(m => m.DestinationId);
            Property(p => p.Owner).HasColumnType("ntext");
            ToTable("MyLodging");
            Property(p => p.Name).HasColumnName("MyName");

           
        }
    }
}
