using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truth.EF.Model
{
    public class Lodging
    {
        public int LodgingId { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int DestinationId { get; set; }
        public Destination Destination { get; set; }
        public List<InternetSpecial> InternetSpecials { get; set; }
    }
}
