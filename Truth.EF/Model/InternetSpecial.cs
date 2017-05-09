using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truth.EF.Model
{
    public class InternetSpecial
    {
        public int InternetSpecialId { get; set; }
        public int Nights { get; set; }
        public decimal CostUSD { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int AccommodationId { get; set; }
        public Lodging Accommodation { get; set; }
    }
}
