using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    class VehicleStateRented : VehicleState
    {
        public override void RentVehicle(Activity activity)
        {
            Console.WriteLine("Vozilo je iznajmljeno i ne može se iznajmiti!");
        }

        public override void ReturnVehicle(Activity activity)
        {
            bool availableSpot = false;
            Rental rental = null;
            LocationCapacity locationCapacity = null;
            int rentalIndex = -1;
            ActivityExecutor activityExecutor = new ActivityExecutor();

            foreach (Rental item in Database.GetInstance().ActiveRentals)
            {
                if (activity.UserId == item.PersonId && activity.VehicleTypeId == item.Vehicle.VehicleTypeId)
                {
                    rental = item;
                    rentalIndex = Database.GetInstance().ActiveRentals.IndexOf(item);
                }
            }

            foreach (LocationCapacity item in Database.GetInstance().LocationCapacities)
            {
                if (activity.LocationId == item.LocationId && activity.VehicleTypeId == item.VehicleTypeId && (item.AvailableVehicleSpots - item.AvailableVehiclesNumber) > 0)
                {
                    locationCapacity = item;
                    availableSpot = true;
                }
            }

            if (rental != null)
            {
                if ((activity.Kms - rental.Vehicle.TotalKms) <= (activityExecutor.GetVehicleType(rental.Vehicle).Range))
                {
                    if (availableSpot)
                    {
                        if (activity.Kms > rental.Vehicle.TotalKms)
                        {
                            //Bill bill = activityExecutor.CreateBill(rental, activity);
                            var create = new CreateBillHandler();
                            var print = new PrintBillHandler();
                            print.SetNext(create);
                            Bill bill = print.Handle(activity, rental) as Bill;
                            if (bill == null) Console.WriteLine("Račun nije napravljen! \n *********** \n ");
                            else
                            {
                                Database.GetInstance().Bills.Add(bill);
                                bill.PrintBill();

                                foreach (Location item in Database.GetInstance().Locations)
                                {
                                    if (item.Id == activity.LocationId) item.TotalEarnings += bill.TotalAmount;
                                }

                                foreach (LocationCapacity item in Database.GetInstance().LocationCapacities)
                                {
                                    if (item.LocationId == activity.LocationId && item.VehicleTypeId == activity.VehicleTypeId) item.AvailableVehiclesNumber += 1;
                                }

                                Database.GetInstance().ActiveRentals.RemoveAt(rentalIndex);
                                rental.ReturnTime = activity.Time;
                                rental.Duration = (int)Math.Round((activity.Time - rental.Time).TotalHours, MidpointRounding.AwayFromZero);
                                rental.Earning = bill.TotalAmount;
                                Database.GetInstance().CompletedRentals.Add(rental);

                                UpdateVehicle(rental.Vehicle, activity);

                                Console.WriteLine($"Vozilo s ID-om {rental.Vehicle.Id} koje je tipa {rental.Vehicle.VehicleTypeId} vratio je korisnik s ID-om {activity.UserId} na lokaciju {rental.Vehicle.LastLocation}. \n *************** \n");
                            }
                        }
                        else Console.WriteLine($"Broj kilometara ne može biti manje/jednako od dosad prijeđenih kilometara vozila! ({rental.Vehicle.TotalKms})");
                    }
                    else Console.WriteLine($"Na lokaciji s ID-om {activity.LocationId} nema mjesta za vozilo tipa {activity.VehicleTypeId}. Vozilo nije vraćeno! \n ************ \n ");
                }
                else Console.WriteLine("Vozilo tog tipa nema takav domet! Neispravan unos! \n ********** \n ");
            }
            else Console.WriteLine($"Korisnik s ID-om {activity.UserId} nije iznajmio vozilo tipa {activity.VehicleTypeId}! \n ************* \n ");
        }

        private void UpdateVehicle(Vehicle vehicle, Activity activity)
        {
            int drainBattery;
            float percentageRange;
            ActivityExecutor activityExecutor = new ActivityExecutor();
            foreach (Vehicle item in Database.GetInstance().Vehicles)
            {
                if (vehicle.Id == item.Id)
                {
                    item.ChargeStartingTime = activity.Time;
                    item.Rented = false;
                    item.TotalKms = activity.Kms;
                    item.LastLocation = activity.LocationId;

                    percentageRange = (activity.Kms / activityExecutor.GetVehicleType(vehicle).Range) * 100;
                    drainBattery = (int)Math.Round(percentageRange, MidpointRounding.AwayFromZero);
                    //Console.WriteLine("praznjenje baterije %" + drainBattery); za kolko se prazni baterija
                    if ((item.Charge - drainBattery) < 0) item.Charge = 0;
                    else item.Charge -= drainBattery;

                    item.TransitionTo(new VehicleStateCharging());
                }
            }
        }

    }
}
