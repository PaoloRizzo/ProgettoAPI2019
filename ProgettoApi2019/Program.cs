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

        static void Main(string[] args)
        {
            relazioni = new Dictionary<string, Dictionary<string, List<string>>>();
            string[] s = null;
            try
            {
                s = File.ReadAllLines("input.txt");
            }
            catch
            {
                Console.WriteLine("Input file not found");
                Console.ReadKey();
                return;
            }

            foreach(string s2 in s)
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

            Report();
            Console.ReadKey();
        }

        private static void Report()
        {
            foreach (var x in relazioni.Keys)
            {
                Console.Write(x);

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

                champions.Sort();

                foreach(var x2 in champions)
                {
                    Console.Write(" " + x2);
                }

                Console.Write(" " + max + ";" + '\n');
            }
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
