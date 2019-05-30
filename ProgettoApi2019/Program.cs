using System;
using System.Collections.Generic;
using System.IO;

namespace ProgettoApi2019
{
    class Program
    {
        // a è amico di b

                                //amico_di          //b       //lista di a
        public static Dictionary<string, Dictionary<string, List<string> >> relazioni;

        public static Random rnd;

        static void Main(string[] args)
        {
            //args = new string[] { "-g", "100", "150000"};

            if (args.Length>0)
            {
                if (args[0] == "-g")
                {
                    Generatore(args);
                    return;
                }
            }

            Solve("input.txt");
            ConsoleStampaConInterazianeUtente(Report());
        }

        private static void ConsoleStampaConInterazianeUtente(string v)
        {
            Console.Write(v);
            Console.ReadKey();
        }

        private static void Solve(string v)
        {

            relazioni = new Dictionary<string, Dictionary<string, List<string>>>();
            string[] s = null;
            try
            {
                s = File.ReadAllLines(v);
            }
            catch
            {
                ConsoleStampaConInterazianeUtente("Input file not found");
                return;
            }

            foreach (string s2 in s)
            {
                if (s2.StartsWith("addrel"))
                {
                    AddRel(s2);
                }
                else if (s2.StartsWith("addent"))
                {
                    AddEnt(s2);
                }
                else if (s2.StartsWith("delent"))
                {
                    DelEnt(s2);
                }
                else if (s2.StartsWith("delrel"))
                {
                    DelRel(s2);
                }
                else
                {
                    Console.WriteLine("UNEXPECTED INPUT: " + s2);
                }
            }

        }

        private static void Generatore(string[] args)
        {
            //=> syntax    .exe -g [NUM_TEST] [AVARAGE_LINES_PER_TEST]
            //=> example   .exe -g 100 150


            if (args.Length < 3)
            {
                ConsoleStampaConInterazianeUtente("Need more arguments");
                return;
            }

            rnd = new Random();
            CreaDirectory();

            int n_test = Convert.ToInt32(args[1]);
            int avg_lines = Convert.ToInt32(args[2]);
            for(int i=0; i< n_test; i++)
            {
                GeneraTest(avg_lines);
            }

            ConsoleStampaConInterazianeUtente(n_test + " generated.");
            return;

        }

        private static void CreaDirectory()
        {
            ProvaCreaDirectory("o");
            ProvaCreaDirectory("i");
        }

        private static void ProvaCreaDirectory(string v)
        {
            if (Directory.Exists(v))
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(v);
            }
            catch
            {
                ;
            }
        }

        private static void GeneraTest(int avg_lines)
        {
            List<string> L = new List<string>();
            for (int i=0; i<avg_lines; i++)
            {
                L.Add(GeneraLinea());
            }

            string output_path = GetOutputPath();
            File.WriteAllLines("i/" + output_path, L);
            Solve("i/" + output_path);
            Save("o/" + output_path);
        }

        private static void Save(string v)
        {
            string output_content = Report();
            File.WriteAllText(v, output_content);
        }

        private static string GetOutputPath()
        {
            return GetDataOra() + ".txt";
        }

        private static string GetDataOra()
        {
            return DateTime.Now.Year + "_" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Day.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Hour.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Minute.ToString().PadLeft(2, '0') + "_" + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Millisecond.ToString().PadLeft(3, '0');
        }

        private static string GeneraLinea()
        {
            int choice = rnd.Next(0, 100);
            if (choice >= 0 && choice <= 25)
                return GeneraAddEnt();
            else if (choice > 25 && choice <= 50)
                return GeneraDelEnt();
            else if (choice > 50 && choice <= 85)
                return GeneraAddRel();
            else if (choice > 85 && choice <= 100)
                return GeneraDelRel();
            else
                throw new Exception();
        }

        private static string GeneraDelRel()
        {
            char carattere1 = RandomLettera();
            char carattere2 = RandomLettera();
            char carattere3 = RandomLettera();
            return "delrel \"" + carattere1 + "\" \"" + carattere2 + "\" \"" + carattere3 + "\"";
        }

        private static string GeneraAddRel()
        {
            char carattere1 = RandomLettera();
            char carattere2 = RandomLettera();
            char carattere3 = RandomLettera();
            return "addrel \"" + carattere1 + "\" \"" + carattere2 + "\" \"" + carattere3 + "\"";
        }

        private static char RandomLettera()
        {
            int lettera = rnd.Next(0, 26);
            lettera += 'a';
            char carattere = (char)lettera;
            return carattere;
        }

        private static string GeneraDelEnt()
        {
            char carattere = RandomLettera();
            return "delent \"" + carattere + "\"";
        }

        private static string GeneraAddEnt()
        {
            int lettera = rnd.Next(0, 26);
            lettera += 'a';
            char carattere = (char)lettera;
            return "addent \""+carattere+"\"";
        }

        private static string Report()
        {
            string output = "";

            List<string> r = new List<string>();
            foreach (var x in relazioni.Keys)
            {
                r.Add(x);
            }
            r.Sort();

            foreach (var x in r)
            {
                List<string> champions = new List<string>();
                int max = -1;

                foreach (var x2 in relazioni[x].Keys)
                {
                    if (relazioni[x][x2].Count>max)
                    {
                        max = relazioni[x][x2].Count;
                        champions.Clear();
                        champions.Add(x2);
                    }
                    else if (relazioni[x][x2].Count == max)
                    {
                        champions.Add(x2);
                    }
                }

                if (max > 0)
                {
                    champions.Sort();

                    output += x;
                    foreach (var x2 in champions)
                    {
                        output += (" " + x2);
                    }

                    output += (" " + max + ";" + '\n');
                }
            }

            return output;
        }

        private static void DelRel(string s2)
        {
            string[] s = s2.Split(' ');
            if (relazioni.ContainsKey(s[3]))
            {
                if (relazioni[s[3]].ContainsKey(s[2]))  
                {
                    if (relazioni[s[3]][s[2]].Contains(s[1]))
                    {
                        relazioni[s[3]][s[2]].Remove(s[1]);
                    }
                    else
                    {
                        //done
                    }
                }
                else
                {
                    //done
                }
            }
            else
            {
                //done
            }
        }

        private static void DelEnt(string s2)
        {
            string[] s = s2.Split(' ');
            foreach (var x in relazioni.Keys)
            {
                relazioni[x].Remove(s[1]);

                foreach(var x2 in relazioni[x].Keys)
                {
                    relazioni[x][x2].Remove(s[1]);
                }
            }
        }

        private static void AddEnt(string s2)
        {
            //done
        }

        private static void AddRel(string s2)
        {
            string[] s = s2.Split(' ');
            if (!relazioni.ContainsKey(s[3]))
            {
                relazioni.Add(s[3], new Dictionary<string, List<string>>());
            }

            if (!relazioni[s[3]].ContainsKey(s[2]))
            {
                relazioni[s[3]].Add(s[2], new List<string>());
            }

            if (!relazioni[s[3]][s[2]].Contains(s[1]))
            {
                relazioni[s[3]][s[2]].Add(s[1]);
            }
        }
    }
}
