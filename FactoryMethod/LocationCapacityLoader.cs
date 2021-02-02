using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dkoprek_zadaca_3
{
    class LocationCapacityLoader : Loader
    {
        List<Object> LocationCapacities = new List<Object>();
        string FileLocation;
        public List<object> LoadDataFromFile(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-k")
                {
                    FileLocation = args[i + 1];
                }
            }

            if (File.Exists(path: FileLocation))
            {
                string[] TxtFileLines = System.IO.File.ReadAllLines(path: FileLocation);
                for (int i = 0; i < TxtFileLines.Length; i++)
                {
                    string[] vars = TxtFileLines[i].Split(';');
                    if (Int32.TryParse(vars[0], out _) && Int32.TryParse(vars[1], out _) && Int32.TryParse(vars[2], out _) && Int32.TryParse(vars[3], out _)) //check
                    {
                        LocationCapacity locationCapacity = new LocationCapacity();
                        locationCapacity.LocationId = Int32.Parse(vars[0]);
                        locationCapacity.VehicleTypeId = Int32.Parse(vars[1]);
                        locationCapacity.AvailableVehicleSpots = Int32.Parse(vars[2]);
                        locationCapacity.AvailableVehiclesNumber = Int32.Parse(vars[3]);
                        locationCapacity.NumberOfBrokenVehicles = 0;
                        LocationCapacities.Add(locationCapacity as Object);
                    }
                    else Console.WriteLine("Greška u " + i + ". redu datoteke" + FileLocation + ". Nije učitan taj red.");
                }
            }
            else Console.WriteLine("Ne postoji datoteka " + FileLocation);
            return LocationCapacities;
        }
    }
}
