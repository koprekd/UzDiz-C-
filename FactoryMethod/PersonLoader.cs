using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dkoprek_zadaca_3
{
    class PersonLoader : Loader
    {
        List<Object> Persons = new List<Object>();
        string FileLocation;
        public List<object> LoadDataFromFile(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-o")
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
                        Person person = new Person();
                        person.Id = Int32.Parse(vars[0]);
                        person.FirstName = vars[1];
                        vars[2] = vars[2].Trim();
                        if (vars[2] == "0") person.Contract = false;
                        else if (vars[2] == "1") person.Contract = true;
                        else
                        {
                            Console.WriteLine("Greška kod parametra 'Ugovor' " + i + ". redu datoteke" + FileLocation + ". Nepoznato stanje ugovora.");
                        }
                        person.NumberOfMalfunctionedVehiclesReturned = 0;
                        person.Debt = 0.00;
                        Persons.Add(person as Object);
                    }
                    else Console.WriteLine("Greška u " + i + ". redu datoteke" + FileLocation + ". Nije učitan taj red.");
                }
            }
            else Console.WriteLine("Ne postoji datoteka " + FileLocation);
            return Persons;
        }
    }
}
