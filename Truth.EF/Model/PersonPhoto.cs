using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Truth.EF.Model
{
    public class PersonPhoto
    {
        [Key]
        public int PersonId { get; set; }
        public byte[] Photo { get; set; }
        public string Caption { get; set; }
        public Person PhotoOf { get; set; }
    }
}
