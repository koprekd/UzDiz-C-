using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace dkoprek_zadaca_3
{
    class ActivityLoader : Loader
    {
        List<Object> Activities = new List<Object>();
        string FileLocation;
        public List<object> LoadDataFromFile(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-s")
                {
                    FileLocation = args[i + 1];
                }
            }

            if (File.Exists(path: FileLocation))
            {
                string[] TxtFileLines = System.IO.File.ReadAllLines(path: FileLocation);

                for (int i = 0; i < TxtFileLines.Length; i++)
                {
                    if(ParseStringToActivity(TxtFileLines[i]) != null)
                    {
                        this.Activities.Add(ParseStringToActivity(TxtFileLines[i], i));
                    }                  
                }
            }
            else Console.WriteLine("Ne postoji datoteka " + FileLocation);
            return Activities;
        }

        public Object ParseStringToActivity(string line, int i=-1)
        {
            string error;
            if(i==-1)
            {
                error = "Pogreška u sintaksi aktivnosti!";
            }
            else
            {
                error = "Greška u " + i + ". redu datoteke" + FileLocation + ". Nije učitan red: \n" + line;
            }

            string[] vars = line.Split(';');

            if (Int32.TryParse(vars[0], out _))
            {
                if (Int32.Parse(vars[0]) >= 1 && Int32.Parse(vars[0]) <= 4)
                {
                    if (vars.Length == 7) //ima problem
                    {
                        Activity activity = new Activity();
                        string time = vars[1].Trim();

                        if (time.Length > 2)
                        {
                            time = time.Substring(1, time.Length - 2);
                            if (Int32.TryParse(vars[0], out _) && DateTime.TryParse(time, out _) && Int32.TryParse(vars[2], out _) && Int32.TryParse(vars[3], out _)
                                && Int32.TryParse(vars[4], out _) && Int32.TryParse(vars[5], out _))
                            {
                                activity.Id = 41;
                                activity.Time = DateTime.Parse(time);
                                activity.UserId = Int32.Parse(vars[2]);
                                activity.LocationId = Int32.Parse(vars[3]);
                                activity.VehicleTypeId = Int32.Parse(vars[4]);
                                activity.Kms = Int32.Parse(vars[5]);
                                activity.Malfunction = vars[6];
                                return activity as Object;
                            }
                            else Console.WriteLine(error);
                        }
                        else Console.WriteLine(error);
                    }

                    else if (vars.Length == 6) //ima zapis kilometara
                    {
                        Activity activity = new Activity();
                        string time = vars[1].Trim();

                        if (time.Length > 2)
                        {
                            time = time.Substring(1, time.Length - 2);
                            if (Int32.TryParse(vars[0], out _) && DateTime.TryParse(time, out _) && Int32.TryParse(vars[2], out _) && Int32.TryParse(vars[3], out _)
                                && Int32.TryParse(vars[4], out _) && Int32.TryParse(vars[5], out _))
                            {
                                activity.Id = Int32.Parse(vars[0]);
                                activity.Time = DateTime.Parse(time);
                                activity.UserId = Int32.Parse(vars[2]);
                                activity.LocationId = Int32.Parse(vars[3]);
                                activity.VehicleTypeId = Int32.Parse(vars[4]);
                                activity.Kms = Int32.Parse(vars[5]);
                                return activity as Object;
                            }
                            else Console.WriteLine(error);
                        }
                        else Console.WriteLine(error);
                    }

                    else if (vars.Length == 5) //nema zapis kilometara
                    {
                        Activity activity = new Activity();
                        string time = vars[1].Trim();

                        if (time.Length > 2)
                        {
                            time = time.Substring(1, time.Length - 2);
                            if (Int32.TryParse(vars[0], out _) && DateTime.TryParse(time, out _) && Int32.TryParse(vars[2], out _) && Int32.TryParse(vars[3], out _)
                                && Int32.TryParse(vars[4], out _))
                            {
                                activity.Id = Int32.Parse(vars[0]);
                                activity.Time = DateTime.Parse(time);
                                activity.UserId = Int32.Parse(vars[2]);
                                activity.LocationId = Int32.Parse(vars[3]);
                                activity.VehicleTypeId = Int32.Parse(vars[4]);
                                return activity as Object;
                            }
                            else Console.WriteLine(error);
                        }
                        else Console.WriteLine(error);
                    }
                }
                else if (Int32.Parse(vars[0]) == 5) //id = 5, učitavanje datoteke
                {
                    if (vars.Length == 2)
                    {
                        Activity activity = new Activity();
                        if (Int32.TryParse(vars[0], out _))
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FileName = vars[1];
                            return activity as Object;
                        }
                        else Console.WriteLine(error);
                    }
                    else Console.WriteLine(error);
                }
                else if (Int32.Parse(vars[0]) == 6)
                {
                    Activity activity = new Activity();
                    if (vars.Length == 2)
                    {
                        vars[1] = vars[1].Trim();
                        string[] commands = vars[1].Split(' ');
                        if (commands[0] == "struktura" && commands.Length == 1) //struktura tvrtke
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.UnitId = -1;
                            return activity as Object;
                        }
                        else if (commands[0] == "struktura" && Int32.TryParse(commands[1], out _) && commands.Length == 2) //struktura podjedinice
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.UnitId = Int32.Parse(commands[1]);
                            return activity as Object;
                        }
                        else if (commands[0] == "struktura" && commands[1] == "stanje" && commands.Length == 2) //stanje tvrtke
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.SecondStringArgument = commands[1];
                            activity.UnitId = -1;
                            return activity as Object;
                        }
                        else if (commands[0] == "struktura" && commands[1] == "stanje" && Int32.TryParse(commands[2], out _) && commands.Length == 3)
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.SecondStringArgument = commands[1];
                            activity.UnitId = Int32.Parse(commands[2]);
                            return activity as Object;
                        }
                        else Console.WriteLine("Neispravni parametri za aktivnost 6!");
                    }
                    else Console.WriteLine("Neispravni parametri za aktivnost 6!");
                }
                else if (Int32.Parse(vars[0]) == 7)
                {
                    Activity activity = new Activity();
                    if (vars.Length == 2)
                    {
                        vars[1] = vars[1].Trim();
                        string[] commands = vars[1].Split(' ');
                        if (commands[0] == "struktura" && commands.Length == 3) //struktura tvrtke
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            if (DateTime.TryParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = -1;
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 7!");
                        }
                        else if (commands[0] == "struktura" && commands.Length == 4 && Int32.TryParse(commands[3], out _))
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            if (DateTime.TryParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = Int32.Parse(commands[3]);
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 7!");
                        }
                        else if (commands[0] == "struktura" && commands[1] == "najam" && commands.Length == 4)
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.SecondStringArgument = commands[1];
                            if (DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[3], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[3], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = -1;
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 7!");
                        }
                        else if (commands[0] == "struktura" && commands[1] == "najam" && Int32.TryParse(commands[4], out _) && commands.Length == 5)
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.SecondStringArgument = commands[1];
                            if (DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[3], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[3], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = Int32.Parse(commands[4]);
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 7!");
                        }
                        else if (commands[0] == "struktura" && commands[1] == "najam" && commands[2] == "zarada" && commands.Length == 5)
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.SecondStringArgument = commands[1];
                            activity.ThirdStringArgument = commands[2];
                            if (DateTime.TryParseExact(commands[3], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[4], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[3], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[4], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = -1;
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 7!");
                        }
                        else if (commands[0] == "struktura" && commands[1] == "najam" && commands[2] == "zarada" && Int32.TryParse(commands[5], out _) && commands.Length == 6)
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.SecondStringArgument = commands[1];
                            activity.ThirdStringArgument = commands[2];
                            if (DateTime.TryParseExact(commands[3], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[4], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[3], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[4], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = Int32.Parse(commands[5]);
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 7!");
                        }
                        else Console.WriteLine("Neispravni parametri za aktivnost 7!");
                    }
                    else Console.WriteLine("Neispravni parametri za aktivnost 7!");
                }
                else if (Int32.Parse(vars[0]) == 8)
                {
                    Activity activity = new Activity();
                    if (vars.Length == 2)
                    {
                        vars[1] = vars[1].Trim();
                        string[] commands = vars[1].Split(' ');
                        if (commands[0] == "struktura" && commands.Length == 3) //struktura tvrtke
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            if (DateTime.TryParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = -1;
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 8!");
                        }
                        else if (commands[0] == "struktura" && commands.Length == 4 && Int32.TryParse(commands[3], out _))
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            if (DateTime.TryParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = Int32.Parse(commands[3]);
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 8!");
                        }
                        else if (commands[0] == "struktura" && (commands[1] == "racuni" || commands[1] == "računi") && commands.Length == 4)
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.SecondStringArgument = commands[1];
                            if (DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[3], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[3], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = -1;
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 8!");
                        }
                        else if (commands[0] == "struktura" && (commands[1] == "racuni" || commands[1] == "računi") && Int32.TryParse(commands[4], out _) && commands.Length == 5)
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.FirstStringArgument = commands[0];
                            activity.SecondStringArgument = commands[1];
                            if (DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[3], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[3], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                activity.UnitId = Int32.Parse(commands[4]);
                                return activity as Object;
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 8!");
                        }
                        else Console.WriteLine("Neispravni parametri za aktivnost 8!");
                    }
                    else Console.WriteLine("Neispravni parametri za aktivnost 8!");
                }
                else if (Int32.Parse(vars[0]) == 0) //id = 0, prekid
                {
                    Activity activity = new Activity();
                    string time = vars[1].Trim();

                    if (time.Length > 2)
                    {
                        //time = time.Substring(1, time.Length - 2);
                        if (Int32.TryParse(vars[0], out _) && DateTime.TryParse(time, out _))
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.Time = DateTime.Parse(time);
                            return activity as Object;
                        }
                        else Console.WriteLine(error);
                    }
                    else Console.WriteLine(error);
                }
                else if (Int32.Parse(vars[0]) == 9)
                {
                    Activity activity = new Activity();
                    if (vars.Length == 1)
                    {
                        vars[0] = vars[0].Trim();
                        if(Int32.TryParse(vars[0], out _)) 
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            return activity as Object;
                        }
                        else Console.WriteLine("Neispravni parametri za aktivnost 9");
                    }
                    else Console.WriteLine("Neispravni parametri za aktivnost 9");
                }
                else if (Int32.Parse(vars[0]) == 10)
                {
                    Activity activity = new Activity();
                    if (vars.Length == 2)
                    {
                        vars[1] = vars[1].Trim();
                        string[] commands = vars[1].Split(' ');
                        if (Int32.TryParse(commands[0], out _) && commands.Length == 3)
                        {
                            activity.Id = Int32.Parse(vars[0]);
                            activity.UserId = Int32.Parse(commands[0]);
                            if (DateTime.TryParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
                                && DateTime.TryParseExact(commands[2], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                            {
                                activity.DateFrom = DateTime.ParseExact(commands[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                activity.DateUntil = DateTime.ParseExact(commands[2], "dd.MM.yyyy", CultureInfo.CurrentCulture);
                                if (DateTime.Compare(activity.DateFrom, activity.DateUntil) < 0) return activity as Object;
                                else
                                {
                                    Console.WriteLine("Neispravni parametri za aktivnost 10! Datum 'od' ne može biti kasniji od datuma 'do'!");
                                    return null;
                                }
                            }
                            else Console.WriteLine("Neispravni parametri za aktivnost 10!");
                        }
                    }
                    else Console.WriteLine("Neispravni parametri za aktivnost 10!");
                }
                else if (Int32.Parse(vars[0]) == 11)
                {
                    Activity activity = new Activity();
                    vars[1] = vars[1].Trim();
                    string[] commands = vars[1].Split(' ');
                    commands[1] = commands[1].Replace(',', '.');
                    if (Int32.TryParse(commands[0], out _) && double.TryParse(commands[1], out _))
                    {
                        activity.Id = Int32.Parse(vars[0]);
                        activity.UserId = Int32.Parse(commands[0]);
                        activity.Amount = double.Parse(commands[1]);
                        return activity;
                    }
                    else Console.WriteLine("Neispravni parametri za aktivnost 11!");
                }
                else Console.WriteLine(error);
            }
            else Console.WriteLine("ID mora biti broj!");
            return null;
        }
    }
}
