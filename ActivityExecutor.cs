using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dkoprek_zadaca_3
{
    class ActivityExecutor
    {
        public void Execute(Activity activity)
        {
            switch (activity.Id)
            {
                case 1:
                    ViewAvailableVehiclesOnLocation(activity.VehicleTypeId, activity.LocationId);
                    break;
                case 2:
                    RentAVehicle(activity);
                    break;
                case 3:
                    ViewAvailableVehicleSpotsOnLocation(activity.VehicleTypeId, activity.LocationId);
                    break;
                case 4:
                    ReturnAVehicle(activity);
                    break;
                case 41:
                    ReturnABrokenVehicle(activity);
                    break;
                case 5:
                    FillListOfActivities(activity);
                    break;
                case 6:
                    StatePrint(activity);
                    break;
                case 7:
                    RentEarningsPrint(activity);
                    break;
                case 8:
                    var create = new CreateBillHandler();
                    var print = new PrintBillHandler();
                    print.SetNext(create); //stvaranje ChainOfResponsibility
                    print.Handle(activity, new Rental());
                    break;
                case 9:
                    PrintUserFinancialState();
                    break;
                case 10:
                    PrintUserBills(activity);
                    break;
                case 11:
                    PayDebt(activity);
                    break;
                default:
                    Console.WriteLine("Aktivnost s tim ID-em nije definirana! \n ************ \n");
                    break;
            }
        }

        private void ViewAvailableVehiclesOnLocation(int vehicleId, int locationId)
        {
            bool found = false, vehicleFound = false, locationFound = false;
            string vehicle = "", 
                location = "";

            foreach (VehicleType item in Database.GetInstance().VehicleTypes)
            {
                if (item.Id == vehicleId)
                {
                    vehicle = item.VehicleName;
                    vehicleFound = true;
                }
            }

            foreach (Location item in Database.GetInstance().Locations)
            {
                if (item.Id == locationId)
                {
                    location = item.Name;
                    locationFound = true;
                }
            }

            foreach (LocationCapacity item in Database.GetInstance().LocationCapacities)
            {
                if (item.LocationId == locationId && item.VehicleTypeId == vehicleId)
                {
                    found = true;
                    Console.WriteLine($"Na lokaciji \"{location}\" dostupno je {item.AvailableVehiclesNumber} vozila vrste \"{vehicle}\" \n ********** \n");
                }
            }
            if (!vehicleFound) Console.WriteLine("Nije pronađeno vozilo s tim ID-em");
            if (!locationFound) Console.WriteLine("Nije pronađena lokacija s tim ID-em");
            if (!found) Console.WriteLine("Neuspješna pretraga \n ************ \n");
        }

        private void ViewAvailableVehicleSpotsOnLocation(int vehicleId, int locationId)
        {
            bool found = false, vehicleFound = false, locationFound = false;
            string vehicle = "",
                location = "";

            foreach (VehicleType item in Database.GetInstance().VehicleTypes)
            {
                if (item.Id == vehicleId)
                {
                    vehicle = item.VehicleName;
                    vehicleFound = true;
                }
            }

            foreach (Location item in Database.GetInstance().Locations)
            {
                if (item.Id == locationId)
                {
                    location = item.Name;
                    locationFound = true;
                }
            }

            foreach (LocationCapacity item in Database.GetInstance().LocationCapacities)
            {
                if (item.LocationId == locationId && item.VehicleTypeId == vehicleId)
                {
                    found = true;
                    Console.WriteLine($"Na lokaciji \"{location}\" dostupno je {item.AvailableVehicleSpots} mjesta za vozila vrste \"{vehicle}\" \n *********** \n");
                }
            }
            if (!vehicleFound) Console.WriteLine("Nije pronađeno vozilo s tim ID-em");
            if (!locationFound) Console.WriteLine("Nije pronađena lokacija s tim ID-em");
            if (!found) Console.WriteLine("Neuspješna pretraga");
        }

        private void RentAVehicle(Activity activity)
        {
            bool alreadyRented = false, isAvailable = false, inDebt = true;
            List<Vehicle> potentialRentals = new List<Vehicle>();

            foreach (Rental item in Database.GetInstance().ActiveRentals) //provjera postojecih najmova korisnika
            {
                if(item.Vehicle.VehicleTypeId == activity.VehicleTypeId && item.PersonId == activity.UserId)
                {
                    Console.WriteLine("Korisnik je već iznajmio taj tip vozila!");
                    alreadyRented = true;
                }
            }

            foreach (LocationCapacity item in Database.GetInstance().LocationCapacities) //provjera ima li dostupnih vozila na lokaciji
            {
                if (item.VehicleTypeId == activity.VehicleTypeId && item.LocationId == activity.LocationId && item.AvailableVehiclesNumber > 0)
                {
                    isAvailable = true;
                }
            }

            foreach (Person item in Database.GetInstance().Persons) //provjera dugovanja
            {
                if (item.Debt < Database.GetInstance().MaxAllowedDebt) inDebt = false;
            }

            if (!alreadyRented && isAvailable && !inDebt) //postoji mogućnosti najma
            {
                foreach (Vehicle item in Database.GetInstance().Vehicles)
                {
                    if (item.Broken == false && item.Rented == false && item.VehicleTypeId == activity.VehicleTypeId && activity.LocationId == item.LastLocation && item.Charge == 100)
                    {
                        potentialRentals.Add(item);
                    }
                }

                if (potentialRentals.Count > 0)
                {

                    if (potentialRentals.Count > 1)
                    {
                        potentialRentals.RemoveAll(x => x.NumberOfRentals > FindLeastRentals(potentialRentals));
                        if (potentialRentals.Count > 1)
                        {
                            potentialRentals.RemoveAll(x => x.TotalKms > FindLeastTotalKms(potentialRentals));
                            if (potentialRentals.Count > 1)
                            {
                                foreach (Vehicle item in potentialRentals)
                                {
                                    if (item.Id == FindMinId(potentialRentals))
                                    {
                                        item.VehicleRentRequest(activity);
                                    }
                                }
                            }
                        }
                        else
                        {
                            potentialRentals[0].VehicleRentRequest(activity);
                        }
                    }
                    else
                    {
                        potentialRentals[0].VehicleRentRequest(activity);
                    }
                }
                else Console.WriteLine($"Na lokaciji {activity.LocationId} nema dostupnih vozila tipa {activity.VehicleTypeId} \n *********** \n");
            }
            else Console.WriteLine($"Na lokaciji {activity.LocationId} nema dostupnih vozila tipa {activity.VehicleTypeId} \n *********** \n");
        }

        private void ReturnAVehicle(Activity activity)
        {
            bool availableSpot = false;
            Rental rental = null;
            LocationCapacity locationCapacity = null;
            int rentalIndex = -1;

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
                if ((activity.Kms - rental.Vehicle.TotalKms) <= (GetVehicleType(rental.Vehicle).Range))
                {
                    if (availableSpot)
                    {
                        if (activity.Kms > rental.Vehicle.TotalKms) 
                        {
                            //Bill bill = CreateBill(rental, activity);
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
                                rental.Duration = (int) Math.Round((activity.Time - rental.Time).TotalHours, MidpointRounding.AwayFromZero);
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

        private void ReturnABrokenVehicle (Activity activity)
        {
            bool availableSpot = false;
            Rental rental = null;
            LocationCapacity locationCapacity = null;
            int rentalIndex = -1;

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
                    availableSpot = true;
                    locationCapacity = item;
                }
            }

            if (rental != null)
            {
                if ((activity.Kms - rental.Vehicle.TotalKms) <= (GetVehicleType(rental.Vehicle).Range))
                {
                    if (availableSpot)
                    {
                        if (activity.Kms > rental.Vehicle.TotalKms)
                        {
                            //Bill bill = CreateBill(rental, activity);
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
                                    if (locationCapacity.LocationId == activity.LocationId && locationCapacity.VehicleTypeId == activity.VehicleTypeId) item.NumberOfBrokenVehicles += 1;
                                }

                                Database.GetInstance().ActiveRentals.RemoveAt(rentalIndex);
                                rental.ReturnTime = activity.Time;
                                rental.Duration = (int)Math.Round((activity.Time - rental.Time).TotalHours, MidpointRounding.AwayFromZero);
                                rental.Earning = bill.TotalAmount;
                                Database.GetInstance().CompletedRentals.Add(rental);

                                UpdateBrokenVehicle(rental.Vehicle, activity);

                                foreach (Person item in Database.GetInstance().Persons)
                                {
                                    if (item.Id == rental.PersonId) item.NumberOfMalfunctionedVehiclesReturned += 1;
                                }

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

        private void FillListOfActivities(Activity activity)
        {
            string FileLocation = activity.FileName;
            ActivityLoader loader = new ActivityLoader();
            Console.WriteLine($"Učitavanje aktivnosti iz datoteke {FileLocation}");
            if (File.Exists(path: FileLocation))
            {
                string[] TxtFileLines = System.IO.File.ReadAllLines(path: FileLocation);

                for (int i = 0; i < TxtFileLines.Length; i++)
                {
                    if (loader.ParseStringToActivity(TxtFileLines[i]) != null)
                    {
                        Activity newActivity = loader.ParseStringToActivity(TxtFileLines[i], i) as Activity;
                        Database.GetInstance().Activities.Add(loader.ParseStringToActivity(TxtFileLines[i], i) as Activity);
                        //Console.WriteLine($"Dodaje se aktivnost {newActivity.Id}, vrijeme: {newActivity.Time}");
                    }
                }
            }
            else Console.WriteLine("Ne postoji datoteka " + FileLocation + " \n ************* \n ");
        }

        private void StatePrint(Activity activity)
        {
            if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == null && activity.UnitId == -1)
            {
                DepthIterator iterator = Database.GetInstance().OrganizationTree.CreateIterator() as DepthIterator;
                iterator.currentOrgUnit.PrintStructure();
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == null && activity.UnitId != -1)
            {
                foreach (OrganizationalUnit item in Database.GetInstance().OrganizationalUnits)
                {
                    if (item.Id == activity.UnitId) item.PrintStructure();                    
                }
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == "stanje" && activity.UnitId == -1)
            {
                DepthIterator iterator = Database.GetInstance().OrganizationTree.CreateIterator() as DepthIterator;
                iterator.currentOrgUnit.PrintState();
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == "stanje" && activity.UnitId != -1)
            {
                foreach (OrganizationalUnit item in Database.GetInstance().OrganizationalUnits)
                {
                    if (item.Id == activity.UnitId) item.PrintState();
                }
            }
            else Console.WriteLine("Neispravni parametri aktivnosti!");
        }

        private void RentEarningsPrint(Activity activity)
        {
            if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == null && activity.UnitId == -1)
            {
                Database.GetInstance().OrganizationTree.PrintStructure();
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == null && activity.UnitId != -1)
            {
                foreach (OrganizationalUnit item in Database.GetInstance().OrganizationalUnits)
                {
                    if (item.Id == activity.UnitId) item.PrintStructure();
                }
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == "najam" && activity.ThirdStringArgument == null && activity.UnitId == -1 &&
                DateTime.Compare(activity.DateFrom, activity.DateUntil) < 0)
            {
                Database.GetInstance().OrganizationTree.PrintRentals(activity);
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == "najam" && activity.ThirdStringArgument == null && activity.UnitId != -1 &&
                DateTime.Compare(activity.DateFrom, activity.DateUntil) < 0)
            {
                foreach (OrganizationalUnit item in Database.GetInstance().OrganizationalUnits)
                {
                    if (item.Id == activity.UnitId) item.PrintRentals(activity);
                }
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == "najam" && activity.ThirdStringArgument == "zarada" && activity.UnitId == -1 &&
                DateTime.Compare(activity.DateFrom, activity.DateUntil) < 0)
            {
                Database.GetInstance().OrganizationTree.PrintEarnings(activity);
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == "najam" && activity.ThirdStringArgument == "zarada" && activity.UnitId != -1 &&
                DateTime.Compare(activity.DateFrom, activity.DateUntil) < 0)
            {
                foreach (OrganizationalUnit item in Database.GetInstance().OrganizationalUnits)
                {
                    if (item.Id == activity.UnitId) item.PrintEarnings(activity);
                }
            }
            else Console.WriteLine("Neispravni parametri aktivnosti!");
        }

        private void BillInfoPrint(Activity activity)
        {
            if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == null && activity.UnitId == -1)
            {
                Database.GetInstance().OrganizationTree.PrintStructure();
            }
            else if (activity.FirstStringArgument == "struktura" && activity.SecondStringArgument == null && activity.UnitId != -1)
            {
                foreach (OrganizationalUnit item in Database.GetInstance().OrganizationalUnits)
                {
                    if (item.Id == activity.UnitId) item.PrintStructure();
                }
            }
            else if (activity.FirstStringArgument == "struktura" && (activity.SecondStringArgument == "racuni" || activity.SecondStringArgument == "računi") && activity.ThirdStringArgument == null && activity.UnitId == -1 &&
                DateTime.Compare(activity.DateFrom, activity.DateUntil) < 0)
            {
                Database.GetInstance().OrganizationTree.PrintBills(activity);
            }
            else if (activity.FirstStringArgument == "struktura" && (activity.SecondStringArgument == "racuni" || activity.SecondStringArgument == "računi") && activity.ThirdStringArgument == null && activity.UnitId != -1 &&
                DateTime.Compare(activity.DateFrom, activity.DateUntil) < 0)
            {
                foreach (OrganizationalUnit item in Database.GetInstance().OrganizationalUnits)
                {
                    if (item.Id == activity.UnitId) item.PrintBills(activity);
                }
            }
            else Console.WriteLine("Neispravni parametri aktivnosti!");
        }

        private void PrintUserFinancialState()
        {
            foreach (Person person in Database.GetInstance().Persons)
            {
                if (person.HadRent)
                {
                    Console.WriteLine($"FINANCIJSKO STANJE KORISNIKA {person.FirstName}");
                    Console.WriteLine($"ID: {person.Id}\nIme: {person.FirstName}\nStanje duga: {person.Debt} kn\nDatum posljednjeg najma: {GetLastRental(person).Time}");
                }
            }
        }

        private void PrintUserBills(Activity activity)
        {
            List<Bill> paidForBills = new List<Bill>();
            List<Bill> unpaidForBills = new List<Bill>();
            foreach (Person person in Database.GetInstance().Persons)
            {
                if(person.Id == activity.UserId)
                {
                    foreach (Bill bill in Database.GetInstance().Bills)
                    {
                        if(person.Id == bill.CustomerId)
                        {
                            if (bill.PaidFor) paidForBills.Add(bill);
                            else unpaidForBills.Add(bill);
                        }
                    }
                }
            }
            paidForBills.Sort((x, y) => x.IssueTime.CompareTo(y.IssueTime));
            unpaidForBills.Sort((x, y) => x.IssueTime.CompareTo(y.IssueTime));

            Console.WriteLine($"Neplaćeni računi korisnika {activity.UserId}:");
            if (unpaidForBills.Count == 0) Console.WriteLine("Nema neplaćenih računa.");
            else
            {
                foreach (Bill item in unpaidForBills)
                {
                    item.PrintBill();
                }
            }

            Console.WriteLine($"Plaćeni računi korisnika {activity.UserId}:");
            if (paidForBills.Count == 0) Console.WriteLine("Nema plaćenih računa.");
            else
            {
                foreach (Bill item in paidForBills)
                {
                    item.PrintBill();
                }
            }
        }

        private void PayDebt(Activity activity)
        {
            List<Bill> userBills = new List<Bill>();
            foreach (Person person in Database.GetInstance().Persons)
            {
                if (person.Id == activity.UserId)
                {
                    foreach (Bill bill in Database.GetInstance().Bills)
                    {
                        if (person.Id == bill.CustomerId)
                        {
                            if (!bill.PaidFor) userBills.Add(bill);
                        }
                    }
                }
            }

            userBills.Sort((x, y) => x.IssueTime.CompareTo(y.IssueTime));
            foreach (Bill item in userBills)
            {
                if(activity.Amount >= item.TotalAmount && activity.Amount > 0)
                {
                    item.PaidFor = true;
                    activity.Amount -= item.TotalAmount;
                    foreach (Person person in Database.GetInstance().Persons)
                    {
                        if (person.Id == item.CustomerId) person.Debt -= item.TotalAmount;
                    }
                    Console.WriteLine($"Korisnik {activity.UserId} je otplatio račun {item.Id} izdan dana {item.IssueTime} u iznosu od {item.TotalAmount} kn.");
                }
            }
            Console.WriteLine($"Korisniku {activity.UserId} se vraća {activity.Amount} kn");
        }

        //pomocne metode
        private int FindLeastRentals(List<Vehicle> vehicles)
        {
            int leastRentals = 999999999;
            foreach (Vehicle item in vehicles)
            {
                if (item.NumberOfRentals < leastRentals) leastRentals = item.NumberOfRentals;
            }
            return leastRentals;
        }

        private int FindLeastTotalKms(List<Vehicle> vehicles)
        {
            int leastKms = 999999999;
            foreach (Vehicle item in vehicles) 
            {
                if (item.TotalKms < leastKms) leastKms = item.TotalKms;
            }
            return leastKms;
        }

        private int FindMinId(List<Vehicle> vehicles)
        {
            int minId = 9999999;
            foreach (Vehicle item in vehicles)
            {
                if (item.Id < minId) minId = item.Id;
            }
            return minId;
        }

        private void Rent(Vehicle vehicle, Activity activity)
        {
            Rental rental = new Rental();
            rental.Vehicle = vehicle;
            rental.PersonId = activity.UserId;
            rental.Time = activity.Time;           

            foreach (LocationCapacity item in Database.GetInstance().LocationCapacities)
            {
                if (item.LocationId == activity.LocationId && item.VehicleTypeId == activity.VehicleTypeId) item.AvailableVehiclesNumber -= 1;
            }

            foreach(Vehicle item in Database.GetInstance().Vehicles)
            {
                if(item.Id == vehicle.Id)
                {                   
                    item.Rented = true;
                    item.NumberOfRentals += 1;
                }
            }

            Database.GetInstance().ActiveRentals.Add(rental);
            Console.WriteLine($"Korisnik s ID-om {activity.UserId} je unajmio vozilo s ID-om {rental.Vehicle.Id} koje je tipa {rental.Vehicle.VehicleTypeId} s lokacije {rental.Vehicle.LastLocation} \n ************** \n");
        }

        public Bill CreateBill(Rental rental, Activity activity)
        {
            Bill bill = new Bill();
            PriceList prices = null;
            foreach (PriceList item in Database.GetInstance().PriceLists)
            {
                if (item.VehicleTypeId == rental.Vehicle.VehicleTypeId) prices = item;
            }

            if (prices != null)
            {
                bill.Id = Database.GetInstance().BillCount;
                Database.GetInstance().BillCount += 1;

                bill.RentalFee = prices.PriceRent;
                //Console.WriteLine($"Sati najma: {Math.Round((activity.Time - rental.Time).TotalHours, MidpointRounding.AwayFromZero)}"); broj sati zaokruzen na vise
                bill.HourlyFee = (float)Math.Round(Math.Round((activity.Time - rental.Time).TotalHours, MidpointRounding.AwayFromZero) * prices.PricePerHour, 2);

                bill.DistanceFee = activity.Kms * prices.PricePerKm;

                bill.TotalAmount = bill.RentalFee + bill.HourlyFee + bill.DistanceFee;

                bill.IssueTime = activity.Time;
                bill.CustomerId = activity.UserId;
                bill.RentalLocationId = rental.Vehicle.LastLocation;
                bill.ReturnLocationId = activity.LocationId;

                return bill;
            }
            else
            {
                Console.WriteLine($"Nije pronađen cjenik za tip vozila {rental.Vehicle.VehicleTypeId} \n *********** \n ");
                return null;
            }
        }

        private void UpdateVehicle(Vehicle vehicle, Activity activity)
        {
            int drainBattery;
            float percentageRange;
            foreach (Vehicle item in Database.GetInstance().Vehicles)
            {
                if(vehicle.Id == item.Id)
                {
                    item.ChargeStartingTime = activity.Time;
                    item.Rented = false;
                    item.TotalKms = activity.Kms;
                    item.LastLocation = activity.LocationId;

                    percentageRange = (activity.Kms / GetVehicleType(vehicle).Range) * 100;
                    drainBattery = (int)Math.Round(percentageRange, MidpointRounding.AwayFromZero);
                    //Console.WriteLine("praznjenje baterije %" + drainBattery); za kolko se prazni baterija
                    if ((item.Charge - drainBattery) < 0) item.Charge = 0;
                    else item.Charge -= drainBattery;
                }
            }
        }

        private void UpdateBrokenVehicle(Vehicle vehicle, Activity activity)
        {
            int drainBattery;
            float percentageRange;
            foreach (Vehicle item in Database.GetInstance().Vehicles)
            {
                if (vehicle.Id == item.Id)
                {
                    item.ChargeStartingTime = activity.Time;
                    item.Rented = false;
                    item.TotalKms += activity.Kms;
                    item.LastLocation = activity.LocationId;
                    item.Broken = true;

                    percentageRange = (activity.Kms / GetVehicleType(vehicle).Range) * 100;
                    drainBattery = (int)Math.Round(percentageRange, MidpointRounding.AwayFromZero);
                    //Console.WriteLine("praznjenje baterije %" + drainBattery); za kolko se prazni baterija
                    if ((item.Charge - drainBattery) < 0) item.Charge = 0;
                    else item.Charge -= drainBattery;

                    item.TransitionTo(new VehicleStateBroken());
                }
            }
        }

        public VehicleType GetVehicleType(Vehicle vehicle)
        {
            VehicleType vehicleType = new VehicleType();
            foreach (VehicleType item in Database.GetInstance().VehicleTypes)
            {
                if (vehicle.VehicleTypeId == item.Id) vehicleType = item;
            }
            return vehicleType;
        }

        public void ChargeBattery(Vehicle vehicle, Activity activity)
        {
            int charged = (int)Math.Round(Math.Round((activity.Time - vehicle.ChargeStartingTime).TotalHours, MidpointRounding.AwayFromZero)/this.GetVehicleType(vehicle).BatteryChargeTime * 100);
            if (vehicle.Charge + charged > 100)
            {
                vehicle.Charge = 100;
                vehicle.TransitionTo(new VehicleStateFree());
            }
            else vehicle.Charge += charged;
        }
        public Rental GetLastRental(Person person)
        {
            Rental lastRental = new Rental();
            foreach (Rental item in Database.GetInstance().ActiveRentals)
            {
                if (DateTime.Compare(item.Time, lastRental.Time) > 0 && item.PersonId == person.Id) lastRental = item;
            }

            foreach (Rental item in Database.GetInstance().CompletedRentals)
            {
                if (DateTime.Compare(item.Time, lastRental.Time) > 0 && item.PersonId == person.Id) lastRental = item;
            }
            return lastRental;
        }
    }
}
