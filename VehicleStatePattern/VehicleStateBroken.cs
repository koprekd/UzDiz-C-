using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    class VehicleStateBroken : VehicleState
    {
        public override void RentVehicle(Activity activity)
        {
            Console.WriteLine("Vozilo je neispravno i ne može se iznajmiti!");
        }

        public override void ReturnVehicle(Activity activity)
        {
            Console.WriteLine("Vozilo nije iznajmljeno i ne može se vratiti!");
        }
    }
}
