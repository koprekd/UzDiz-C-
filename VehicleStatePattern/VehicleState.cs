using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    public abstract class VehicleState
    {
        protected Vehicle _vehicleContext;

        public void SetContext(Vehicle vehicleContext)
        {
            this._vehicleContext = vehicleContext;
        }

        public abstract void ReturnVehicle(Activity activity);
        public abstract void RentVehicle(Activity activity);
    }
}
