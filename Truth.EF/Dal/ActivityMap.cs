using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Truth.EF.Model;

namespace Truth.EF.Dal
{
    public class ActivityMap:EntityTypeConfiguration<Activity>
    {
        public ActivityMap()
        {
            this.HasMany(a => a.Trips).WithMany(t => t.Activitys).Map(m =>
            {
                m.ToTable("TripActivities");
                m.MapLeftKey("ActivityId");
                m.MapRightKey("TripIdentifier");
            });
        }
    }
}
