using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    class VehicleStateFree : VehicleState
    {
        public override void RentVehicle(Activity activity)
        {
            Rental rental = new Rental();
            rental.Vehicle = this._vehicleContext;
            rental.PersonId = activity.UserId;
            rental.Time = activity.Time;

            foreach (LocationCapacity item in Database.GetInstance().LocationCapacities)
            {
                if (item.LocationId == activity.LocationId && item.VehicleTypeId == activity.VehicleTypeId) item.AvailableVehiclesNumber -= 1;
            }

            foreach (Vehicle item in Database.GetInstance().Vehicles)
            {
                if (item.Id == this._vehicleContext.Id)
                {
                    item.Rented = true;
                    item.NumberOfRentals += 1;
                    item.TransitionTo(new VehicleStateRented());
                }
            }

            Database.GetInstance().ActiveRentals.Add(rental);
            foreach (Person item in Database.GetInstance().Persons)
            {
                if (activity.UserId == item.Id) item.HadRent = true;
            }
            Console.WriteLine($"Korisnik s ID-om {activity.UserId} je unajmio vozilo s ID-om {rental.Vehicle.Id} koje je tipa {rental.Vehicle.VehicleTypeId} s lokacije {rental.Vehicle.LastLocation} \n ************** \n");
        }

        public override void ReturnVehicle(Activity activity)
        {
            Console.WriteLine("Vozilo nije iznajmljeno i ne može se vratiti!");
        }
    }
}
