using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Truth.EF.Model;
namespace Truth.EF.Dal
{
    public class InternetSpecialMap:EntityTypeConfiguration<InternetSpecial>
    {
        public InternetSpecialMap()
        {
            HasRequired(a => a.Accommodation)
                .WithMany(l => l.InternetSpecials)
                .HasForeignKey(s => s.AccommodationId);
        }
    }
}
