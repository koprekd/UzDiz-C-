using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace dkoprek_zadaca_3
{
    class Program
    {
        static void Main(string[] args)
        {
            args = CheckConfiguration(args);
            if (CheckParameters(args))
            {
                Console.WriteLine("Parametri ispravni!");
                Database database = Database.GetInstance();
                VirtualTime virtualTime = VirtualTime.GetInstance();
                database.LoadData(args);
                virtualTime.CurrentTime = GetTimeParameter(args);
                //PrintDatabase(database);

                if (database.Activities.Count == 0) InteractiveMode();
                else FileMode();
                //PrintDatabase(database);

            }
            else Console.WriteLine("Neispravni parametri");

            Console.WriteLine($"Program je završio s virtualnim vremenom {VirtualTime.GetInstance().CurrentTime}.");
        }

        static void InteractiveMode()
        {
            Console.WriteLine(" #################### \n Ulazim u interaktivni način rada \n ####################");
            while (true)
            {
                Console.WriteLine("-.-.-.-.-.-.-.-.-.-.-.-.-\nUnesite aktivnost: ");

                string input = Console.ReadLine();
                string[] inputVars = input.Split(';');
                if (inputVars.Length < 1) Console.WriteLine("Neispravan unos!");
                else
                {
                    ActivityLoader loader = new ActivityLoader();
                    Activity inputActivity = loader.ParseStringToActivity(input) as Activity;
                    if (inputActivity == null) Console.WriteLine("     ------"); //grešku ispisuje ActivityLoader ako nije uspio stvoriti activity
                    else if (inputActivity.Id == 5)
                    {
                        ActivityExecutor executor = new ActivityExecutor();
                        executor.Execute(inputActivity);
                        FileMode();
                    }
                    else if (inputActivity.Id == 6 || inputActivity.Id == 7 || inputActivity.Id == 8 || inputActivity.Id == 9 || inputActivity.Id == 10 || inputActivity.Id == 11)
                    {
                        Console.WriteLine($"Izvodi se aktivnost {inputActivity.Id}\n       |\n       |\n       V");
                        ActivityExecutor executor = new ActivityExecutor();
                        executor.Execute(inputActivity);
                    }
                    else
                    {
                        if (!CheckNewTime(VirtualTime.GetInstance().CurrentTime, inputActivity.Time)) Console.WriteLine($"Uneseno vrijeme nije moguće! Trenutno vrijeme: {VirtualTime.GetInstance().CurrentTime} \n ********** \n");
                        else
                        {
                            VirtualTime.GetInstance().CurrentTime = inputActivity.Time;
                            if (inputActivity.Id == 0) break;
                            ActivityExecutor executor = new ActivityExecutor();
                            Console.WriteLine($"Izvodi se aktivnost {inputActivity.Id}\n       |\n       |\n       V");
                            executor.Execute(inputActivity);
                            ChargeBatteries(inputActivity);
                        }
                    }
                }
            }
        }

        static void FileMode()
        {
            Console.WriteLine(" #################### \n Ulazim u skupni način rada \n ####################");
            foreach (Activity item in Database.GetInstance().Activities)
            {
              
                if (item.Id == 0) break;                
                else if (item.Id == 6 || item.Id == 7 || item.Id == 8 || item.Id == 9 || item.Id == 10 || item.Id == 11) //aktivnosti koje ne azuriraju virtualno vrijeme
                {
                    Console.WriteLine($"Izvodi se aktivnost {item.Id} iz retka {Database.GetInstance().Activities.IndexOf(item) + 2}\n       |\n       |\n       V");
                    ActivityExecutor executor = new ActivityExecutor();
                    executor.Execute(item);
                    if (Database.GetInstance().Activities.IndexOf(item) == Database.GetInstance().Activities.Count - 1 && item.Id != 0) InteractiveMode();
                }
                else
                {
                    Console.WriteLine($"Izvodi se aktivnost {item.Id} u vremenu {item.Time} iz retka {Database.GetInstance().Activities.IndexOf(item) + 2}\n       |\n       |\n       V");
                    if (!CheckNewTime(VirtualTime.GetInstance().CurrentTime, item.Time)) Console.WriteLine("Vrijeme ove aktivnosti nije moguće! Trenutno vrijeme: " + VirtualTime.GetInstance().CurrentTime + ". Prelazak na sljedeću aktivnost. \n ********** \n");
                    else
                    {
                        VirtualTime.GetInstance().CurrentTime = item.Time;
                        ActivityExecutor executor = new ActivityExecutor();
                        executor.Execute(item);
                        ChargeBatteries(item);
                    }
                    if (Database.GetInstance().Activities.IndexOf(item) == Database.GetInstance().Activities.Count - 1 && item.Id != 0) InteractiveMode();
                }
            }
        }

        static string[] CheckConfiguration(string[] args)
        {
            if (File.Exists(path: args[0]))
            {
                args = GetParametersFromFile(args[0]);
                return args;
            }
            else return args;
        }

        static bool CheckParameters (string[] args)
        {
            bool v = false; bool l = false; bool c = false; bool k = false; bool o = false; bool t = false, os = false, dug = false;
            string auxTime, auxTime2, time, time2, finaltime;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-v") v = true;
                if (args[i] == "-l") l = true;
                if (args[i] == "-c") c = true;
                if (args[i] == "-k") k = true;
                if (args[i] == "-o") o = true;
                if (args[i] == "-os") os = true;
                if (args[i] == "-t")
                {
                    auxTime = args[i + 1];
                    time = auxTime.Substring(1, auxTime.Length - 1);

                    auxTime2 = args[i + 2];
                    time2 = auxTime2.Substring(0, auxTime2.Length - 1);

                    finaltime = time + " " + time2;
                    if (DateTime.TryParseExact(finaltime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) t = true;
                }
                if(args[i] == "-dug")
                {
                    string aux;
                    aux = args[i + 1].Replace(',', '.');
                    if (double.TryParse(aux, out _)) dug = true;
                }
            }

            if (v && l && c && k && o && t && os && dug) return true;
            else return false;
        }

        static void PrintDatabase(Database database)
        {
            Console.WriteLine("-----------OSOBE-----------");
            foreach (Person person in database.Persons)
            {
                Console.WriteLine(person.FirstName + "\n");
            }

            Console.WriteLine("-----------VRSTE VOZILA-----------");
            foreach (VehicleType vehicle in database.VehicleTypes)
            {
                Console.WriteLine(vehicle.VehicleName + "\n");
            }

            Console.WriteLine("-----------AKTIVNOSTI-----------");
            foreach (Activity activity in database.Activities)
            {
                    Console.WriteLine(activity.Id + " " + activity.Time + "\n");
            }

            Console.WriteLine("-----------LOKACIJE-----------");
            foreach (Location location in database.Locations)
            {
                Console.WriteLine(location.Name + " Zarada: " + location.TotalEarnings + "\n");
            }

            Console.WriteLine("-----------KAPACITETI-----------");
            foreach (LocationCapacity item in database.LocationCapacities)
            {
                Console.WriteLine(item.LocationId + " Dostupnih mjesta za vozilo:" + item.AvailableVehicleSpots + " Dostupnih vozila:" + item.AvailableVehiclesNumber + "\n");
            }

            Console.WriteLine("-----------CJENIK-----------");
            foreach (PriceList item in database.PriceLists)
            {
                Console.WriteLine(item.VehicleTypeId + " Cijena najma:" + item.PriceRent + "\n");
            }

            Console.WriteLine("-----------VOZILA-----------");
            foreach (Vehicle item in database.Vehicles)
            {
                Console.WriteLine("ID vozila: " + item.Id + "; ID vrste vozila: " + item.VehicleTypeId + "; Na lokaciji: " + item.LastLocation + " Baterija: " + item.Charge + " Broj najmova: " + item.NumberOfRentals);
            }

            Console.WriteLine("-----------RAČUNI-----------");
            foreach (Bill item in database.Bills)
            {
                Console.WriteLine($"ID računa: {item.Id}; Iznos: {item.TotalAmount}");
                Console.WriteLine($"Ukupan broj računa: {database.BillCount}");
            }

            Console.WriteLine("-----------AKTIVNI NAJMOVI-----------");
            foreach (Rental item in database.ActiveRentals)
            {
                Console.WriteLine($"Vrijeme najma: {item.Time}; Korisnik: {item.PersonId}; Tip vozila: {item.Vehicle.VehicleTypeId}");
            }
        }

        static DateTime GetTimeParameter(string[] args)
        {
            string auxTime, auxTime2, time, time2, finaltime;
            int index = 0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-t") index = i;
            }
            auxTime = args[index + 1];
            time = auxTime.Substring(1, auxTime.Length - 1);

            auxTime2 = args[index + 2];
            time2 = auxTime2.Substring(0, auxTime2.Length - 1);

            finaltime = time + " " + time2;
            return (DateTime.ParseExact(finaltime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        }

        static bool CheckNewTime(DateTime currentVirtualTime, DateTime newVirtualTime)
        {
            if (DateTime.Compare(currentVirtualTime, newVirtualTime) > 0) return false;
            else return true;
        }

        static void ChargeBatteries(Activity activity)
        {
            ActivityExecutor activityExecutor = new ActivityExecutor();
            foreach (Vehicle item in Database.GetInstance().Vehicles)
            {
                if(item.Charge < 100)
                {
                    activityExecutor.ChargeBattery(item, activity);
                }
            }
        }

        static string[] GetParametersFromFile (string fileLocation)
        {
            string[] parameters = new string[30];
            string[] TxtFileLines = System.IO.File.ReadAllLines(path: fileLocation);
            int counter = 0;
            bool tekst = false, cijeli = false, decimala = false;
            for (int i = 0; i < TxtFileLines.Length; i++)
            {
                string[] line = TxtFileLines[i].Split('=');
                if (line[0] == "vozila")
                {
                    parameters[counter] = "-v";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "lokacije")
                {
                    parameters[counter] = "-l";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "cjenik")
                {
                    parameters[counter] = "-c";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "kapaciteti")
                {
                    parameters[counter] = "-k";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "osobe")
                {
                    parameters[counter] = "-o";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "vrijeme")
                {
                    parameters[counter] = "-t";
                    string[] auxTime = line[1].Split();
                    parameters[counter + 1] = "," + auxTime[0];
                    parameters[counter + 2] = auxTime[1] + ",";
                    counter += 3;
                }

                if (line[0] == "struktura")
                {
                    parameters[counter] = "-os";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "aktivnosti")
                {
                    parameters[counter] = "-s";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "dugovanje")
                {
                    parameters[counter] = "-dug";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "izlaz")
                {
                    parameters[counter] = "-iz";
                    parameters[counter + 1] = line[1];
                    counter += 2;
                }

                if (line[0] == "tekst")
                {
                    parameters[counter] = "-tek";
                    parameters[counter + 1] = line[1];
                    tekst = true;
                    counter += 2;
                }

                if (line[0] == "cijeli")
                {
                    parameters[counter] = "-cij";
                    parameters[counter + 1] = line[1];
                    cijeli = true;
                    counter += 2;
                }

                if (line[0] == "decimala")
                {
                    parameters[counter] = "-dec";
                    parameters[counter + 1] = line[1];
                    decimala = true;
                    counter += 2;
                }
            }

            if (!tekst)
            {
                parameters[counter] = "-tek";
                parameters[counter + 1] = "30";
                counter += 2;
            }

            if (!cijeli)
            {
                parameters[counter] = "-cij";
                parameters[counter + 1] = "5";
                counter += 2;
            }

            if (!decimala)
            {
                parameters[counter] = "-dec";
                parameters[counter + 1] = "2";
                counter += 2;
            }
            return parameters;
        }

    }
}
