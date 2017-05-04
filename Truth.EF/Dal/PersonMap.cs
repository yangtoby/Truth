using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Truth.EF.Model;

namespace Truth.EF.Dal
{
    public class PersonMap:EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            this.Property(p => p.SocialSecurityNumber).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            this.Property(p => p.SocialSecurityNumber).IsConcurrencyToken();
        }
    }
}
