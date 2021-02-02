using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    class OrganizationalUnit : OrganizationalUnitInterface, Iterable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public List<int> LocationIds = new List<int>();
        public float TotalEarnings { get; set; }
        public int NumberOfRentalsFrom = 0;
        public int DurationOfAllRentals = 0;

        public List<OrganizationalUnitInterface> SubUnits = new List<OrganizationalUnitInterface>();
        public List<OrganizationalUnitInterface> Locations = new List<OrganizationalUnitInterface>();

        public override void Add(OrganizationalUnitInterface unit)
        {
            if (!unit.IsUnit()) this.Locations.Add(unit);
            else this.SubUnits.Add(unit);
        }

        public override void Remove(OrganizationalUnitInterface unit)
        {
            if (!unit.IsUnit()) this.Locations.Remove(unit);
            else this.SubUnits.Remove(unit);
        }

        public override bool IsUnit()
        {
            return true;
        }
        public override void PrintStructure()
        {
            Console.WriteLine($"Organizacijska jedinica: {this.Name} \n///// Podjedinice jedinice {this.Name}:");

            foreach (OrganizationalUnitInterface item in this.SubUnits)
            {
                item.PrintStructure();
            }

            Console.WriteLine($"///// Lokacije jedinice {this.Name}: ");
            foreach (OrganizationalUnitInterface item in this.Locations)
            {
                item.PrintStructure();
            }
        }

        public override void PrintState()
        {
            Console.WriteLine($"Organizacijska jedinica: {this.Name}");

            Console.WriteLine($"///// Podjedinice jedinice {this.Name}:");

            foreach (OrganizationalUnitInterface item in this.SubUnits)
            {
                item.PrintState();
            }

            Console.WriteLine($"///// Lokacije jedinice {this.Name}: ");
            foreach (OrganizationalUnitInterface item in this.Locations)
            {
                item.PrintState();
            }
        }

        public override void PrintRentals(Activity activity)
        {
            Console.WriteLine($"Organizacijska jedinica: {this.Name} \n///// Podjedinice jedinice {this.Name}:");           
            foreach (OrganizationalUnitInterface item in this.SubUnits)
            {
                item.PrintRentals(activity);
            }

            Console.WriteLine($"///// Lokacije jedinice {this.Name}: ");
            foreach (OrganizationalUnitInterface item in this.Locations)
            {
                item.PrintRentals(activity);
            }
            //PrintRentalInfo();
        }

        public override void PrintEarnings(Activity activity)
        {
            Console.WriteLine($"Organizacijska jedinica: {this.Name} \n///// Podjedinice jedinice {this.Name}:");
            foreach (OrganizationalUnitInterface item in this.SubUnits)
            {
                item.PrintEarnings(activity);
            }

            Console.WriteLine($"///// Lokacije jedinice {this.Name}: ");
            foreach (OrganizationalUnitInterface item in this.Locations)
            {
                item.PrintEarnings(activity);
            }
            //PrintEarningsInfo();
        }

        public override void PrintBills(Activity activity)
        {
            Console.WriteLine($"Organizacijska jedinica: {this.Name} \n///// Podjedinice jedinice {this.Name}:");
            foreach (OrganizationalUnitInterface item in this.SubUnits)
            {
                item.PrintBills(activity);
            }

            Console.WriteLine($"///// Lokacije jedinice {this.Name}: ");
            foreach (OrganizationalUnitInterface item in this.Locations)
            {
                item.PrintBills(activity);
            }
        }

        private void PrintEarningsInfo()
        {
            foreach (OrganizationalUnit item in this.SubUnits)
            {
                this.NumberOfRentalsFrom += item.NumberOfRentalsFrom;
                this.DurationOfAllRentals += item.DurationOfAllRentals;
                this.TotalEarnings += item.TotalEarnings;
            }
            foreach (Location item in this.Locations)
            {
                this.NumberOfRentalsFrom += item.NumberOfRentalsFrom;
                this.DurationOfAllRentals += item.DurationOfAllRentals;
                this.TotalEarnings += item.TotalEarnings;
            }
            Console.WriteLine($"UKUPNO ZA: Org. jedinica: {this.Name}. Broj najmova iz jedinice: {this.NumberOfRentalsFrom}. Ukupno trajanje najmova iz jedinice: {this.DurationOfAllRentals} sati.");
            Console.WriteLine($"UKUPNA ZARADA za jedinicu {this.Name}: {this.TotalEarnings}");
        }

        private void PrintRentalInfo()
        {
            
            foreach (OrganizationalUnit item in this.SubUnits)
            {
                this.DurationOfAllRentals += item.DurationOfAllRentals;
                this.NumberOfRentalsFrom += item.NumberOfRentalsFrom;
            }
            foreach (Location item in this.Locations)
            {
                this.NumberOfRentalsFrom += item.NumberOfRentalsFrom;
                this.DurationOfAllRentals += item.DurationOfAllRentals;
            }
            Console.WriteLine($"UKUPNO ZA: Org. jedinica: {this.Name}. Broj najmova iz jedinice: {this.NumberOfRentalsFrom}. Ukupno trajanje najmova iz jedinice: {this.DurationOfAllRentals} sati.");
        }

        public Iterator CreateIterator() 
        {
            DepthIterator depthIterator = new DepthIterator(this);
            return depthIterator;
        }

    }
}
