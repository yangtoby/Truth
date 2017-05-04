using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Truth.EF.Model;

namespace Truth.EF.Dal
{
    public class TripMap:EntityTypeConfiguration<Trip>
    {
        public TripMap()
        {
            this.HasKey(t => t.Identifier);
            this.Property(t => t.Identifier).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.Property(t => t.RowVersion).IsRowVersion();
        }
    }
}
