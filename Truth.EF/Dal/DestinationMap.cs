using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Truth.EF.Model;

namespace Truth.EF.Dal
{
    public class DestinationMap:EntityTypeConfiguration<Destination>
    {
        public DestinationMap()
        {
            HasKey(d => d.DestinationId);
            Property(d => d.Name).IsRequired();
            Property(d => d.Description).HasMaxLength(500).IsRequired();
            //HasMany(d => d.Lodgings).WithRequired(l => l.Destination).Map(l => l.MapKey("EdstinationId"));
        }
    }
}
