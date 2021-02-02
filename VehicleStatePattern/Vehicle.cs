using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int VehicleTypeId { get; set; }
        public int LastLocation { get; set; }
        public int Charge { get; set; }
        public int TotalKms { get; set; }
        public bool Rented { get; set; }
        public bool Broken { get; set; }
        public int NumberOfRentals { get; set; }
        public DateTime ChargeStartingTime { get; set; }


        private VehicleState _vehicleState = null;

        public Vehicle(VehicleState vehicleState)
        {
            this.TransitionTo(vehicleState);
        }

        public void TransitionTo(VehicleState vehicleState)
        {
            this._vehicleState = vehicleState;
            this._vehicleState.SetContext(this);
        }

        public void VehicleRentRequest(Activity activity)
        {
            this._vehicleState.RentVehicle(activity);
        }

        public void VehicleReturnRequest(Activity activity)
        {
            this._vehicleState.ReturnVehicle(activity);
        }

        public VehicleState GetVehicleState()
        {
            return this._vehicleState;
        }
    }
}
