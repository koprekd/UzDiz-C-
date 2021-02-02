using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dkoprek_zadaca_3
{
    class PriceListLoader : Loader
    {
        List<Object> PriceLists = new List<Object>();
        string FileLocation;
        public List<object> LoadDataFromFile(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-c")
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
                    if (Int32.TryParse(vars[0], out _) && float.TryParse(vars[1], out _) && float.TryParse(vars[2], out _) && float.TryParse(vars[3], out _)) //check
                    {
                        PriceList pricelist = new PriceList();
                        pricelist.VehicleTypeId = Int32.Parse(vars[0]);
                        pricelist.PriceRent = float.Parse(vars[1])/100;
                        pricelist.PricePerHour = float.Parse(vars[2])/100;
                        pricelist.PricePerKm = float.Parse(vars[3])/100;
                        PriceLists.Add(pricelist as Object);
                    }
                    else Console.WriteLine("Greška u " + i + ". redu datoteke" + FileLocation + ". Nije učitan taj red.");
                }
            }
            else Console.WriteLine("Ne postoji datoteka " + FileLocation);
            return PriceLists;
        }
    }
}
