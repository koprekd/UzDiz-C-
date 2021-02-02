using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    public class Rental
    {
        public Vehicle Vehicle { get; set; }
        public int PersonId { get; set; }
        public DateTime Time { get; set; }
        public DateTime ReturnTime { get; set; }
        public int Duration { get; set; }
        public float Earning { get; set; }
    }
}
