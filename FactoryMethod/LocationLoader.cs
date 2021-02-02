using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dkoprek_zadaca_3
{
    class LocationLoader : Loader
    {
        List<Object> Locations = new List<Object>();
        string FileLocation;
        public List<object> LoadDataFromFile(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-l")
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
                    if (Int32.TryParse(vars[0], out _)) //check
                    {
                        Location location = new Location();
                        location.Id = Int32.Parse(vars[0]);
                        location.Name = vars[1];
                        location.Address = vars[2];

                        string[] gps = vars[3].Split(',');
                        if (float.TryParse(gps[0], out _) && float.TryParse(gps[1], out _))
                        {
                            location.Gps[0] = float.Parse(gps[0]);
                            location.Gps[1] = float.Parse(gps[1]);                            
                        }
                        else
                        {
                            Console.WriteLine("Greška u koordinatama u " + i + ". redu. Red nije učitan");
                            continue;
                        }

                        Locations.Add(location as Object);
                    }
                    else Console.WriteLine("Greška u " + i + ". redu datoteke" + FileLocation + ". Nije učitan taj red.");
                }
            }
            else Console.WriteLine("Ne postoji datoteka " + FileLocation);
            return Locations;
        }
    }
}
