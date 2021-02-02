using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    class OrganizationUnitObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SuperiorUnit { get; set; }

        public List<int> LocationIds = new List<int>();
    }
}
