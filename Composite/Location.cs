using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    class Location : OrganizationalUnitInterface
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float[] Gps = new float[2];
        public float TotalEarnings { get; set; }
        public int NumberOfRentalsFrom = 0;
        public int DurationOfAllRentals = 0;

        public override bool IsUnit()
        {
            return false;
        }

        public override void PrintStructure()
        {
            Console.WriteLine($"Lokacija: {this.Name}");
        }

        public override void PrintState()
        {
            Console.WriteLine($"Lokacija: {this.Name}");
            foreach (LocationCapacity item in Database.GetInstance().LocationCapacities)
            {
                if(item.LocationId == this.Id)
                {
                    Console.WriteLine($"~Broj mjesta za tip vozila {item.VehicleTypeId}: {item.AvailableVehicleSpots}");
                    Console.WriteLine($"~Broj dostupnih vozila tipa {item.VehicleTypeId}: {item.AvailableVehiclesNumber}");
                }
            }
        }

        public override void PrintRentals(Activity activity)
        {
            this.NumberOfRentalsFrom = 0;
            this.DurationOfAllRentals = 0;
            foreach (Rental item in Database.GetInstance().CompletedRentals)
            {
                if (item.Vehicle.LastLocation == this.Id && DateTime.Compare(activity.DateFrom, item.Time) < 0 && DateTime.Compare(item.ReturnTime, activity.DateUntil) < 0)
                {
                    this.NumberOfRentalsFrom++;
                    this.DurationOfAllRentals += item.Duration;
                }
            }
            Console.WriteLine($"Lokacija: {this.Name}. Broj najmova s lokacije: {this.NumberOfRentalsFrom}. Ukupno trajanje najmova s lokacije: {this.DurationOfAllRentals} sati.");
        }

        public override void PrintEarnings(Activity activity)
        {
            Console.WriteLine($"Lokacija: {this.Name}. Ukupna zarada lokacije: {this.TotalEarnings}");
        }

        public override void PrintBills(Activity activity)
        {
            Console.WriteLine($"Računi za lokaciju {this.Name}:");
            foreach (Bill item in Database.GetInstance().Bills)
            {
                if (item.RentalLocationId == this.Id && DateTime.Compare(activity.DateFrom, item.IssueTime) < 0 && DateTime.Compare(item.IssueTime, activity.DateUntil) < 0) item.PrintBill();
            }
        }
    }


}
