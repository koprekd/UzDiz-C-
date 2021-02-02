using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    public class LocationCapacity
    {
        public int LocationId { get; set; }
        public int VehicleTypeId { get; set; }
        public int AvailableVehicleSpots { get; set; }
        public int AvailableVehiclesNumber { get; set; }
        public int NumberOfBrokenVehicles { get; set; }
    }
}
