using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public int NumberOfMalfunctionedVehiclesReturned { get; set; }
        public bool Contract { get; set; }
        public double Debt { get; set; }
        public bool HadRent { get; set; }
    }
}
