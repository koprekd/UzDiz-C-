using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    class OrganizationUnitObjectLoader : Loader
    {
        List<Object> OrganizationUnitObjects = new List<Object>();
        string FileLocation;
        public List<object> LoadDataFromFile(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-os")
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

                    string[] locations = null;
                    bool correctLocations = true, locationsExist = false;
                    if (vars.Length == 4)
                    {
                        if (vars[2] == " ") vars[2] = "-1";
                        if (vars[3] != "") 
                        {
                            locationsExist = true;
                            locations = vars[3].Split(','); 
                        }
                    }

                    if (locationsExist)
                    {
                        for (int j = 0; j < locations.Length; j++)
                        {
                            if (!Int32.TryParse(locations[j], out _)) correctLocations = false;
                        }
                    }

                    if (Int32.TryParse(vars[0], out _) && Int32.TryParse(vars[2], out _) && correctLocations) //check
                    {
                        OrganizationUnitObject unit = new OrganizationUnitObject();
                        unit.Id = Int32.Parse(vars[0]);
                        unit.Name = vars[1];
                        unit.SuperiorUnit = Int32.Parse(vars[2]);

                        if (locationsExist)
                        {
                            for (int k = 0; k < locations.Length; k++)
                            {
                                unit.LocationIds.Add(Int32.Parse(locations[k]));
                            }
                        }
                        OrganizationUnitObjects.Add(unit);
                    }
                    else Console.WriteLine("Greška u " + i + ". redu datoteke" + FileLocation + ". Nije učitan taj red.");
                }
            }
            else Console.WriteLine("Ne postoji datoteka " + FileLocation);
            return OrganizationUnitObjects;
        }
    }
}
