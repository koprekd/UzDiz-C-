using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    public class Activity
    {
        //za aktivnosti 1-4
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int VehicleTypeId { get; set; }
        public int Kms { get; set; }

        public string Malfunction { get; set; } //za aktivnost povratka pokvarenog vozila

        public string FileName { get; set; } //za aktivnost 5

        //za aktivnosti 6,7,8
        public string FirstStringArgument { get; set; }
        public string SecondStringArgument { get; set; }
        public string ThirdStringArgument { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
        public int UnitId { get; set; }

        //za aktivnost 11
        public double Amount { get; set; }
    }
}
