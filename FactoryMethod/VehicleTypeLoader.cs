using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dkoprek_zadaca_3
{
    class VehicleTypeLoader : Loader
    {
        List<Object> Vehicles = new List<Object>();
        string FileLocation;
        public List<object> LoadDataFromFile(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-v") 
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
                    if (Int32.TryParse(vars[0], out _) && float.TryParse(vars[2], out _) && float.TryParse(vars[3], out _)) //check
                    {
                        VehicleType vehicle = new VehicleType();
                        vehicle.Id = Int32.Parse(vars[0]);
                        vehicle.VehicleName = vars[1];
                        vehicle.BatteryChargeTime = float.Parse(vars[2]);
                        vehicle.Range = float.Parse(vars[3]);
                        Vehicles.Add(vehicle as Object);
                    }
                    else Console.WriteLine("Greška u " + i + ". redu datoteke" + FileLocation + ". Nije učitan taj red.");
                }
            }
            else Console.WriteLine("Ne postoji datoteka " + FileLocation);
            return Vehicles;
        }
    }
}
