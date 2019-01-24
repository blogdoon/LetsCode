using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge2
{
    /* Tasks
   1: Bug: something odd happens when transfering the last container
   2: As a user I want the load of my ships to be persisted so that I can restart the program and continue from where I left (txt-files is enough)
   3: As a user I want to restrict the load pr ship to a max of 4 containers because ship 1 and 2 have a max capacity of 4
   4: As a user I want to register an unlimitted number of ships and make load transfers between them, so that I can manage my whole fleet
   5: As a user I want to specify individual capacities per ship, so that I can handle ships with diferent capacities
   6: As a user I want to also manage trucks, so that I can use the program to manage load of my trucks as well
   7: As a user I want the offloading from trucks to be restricted to the last load, so that it is not possible to unload unreachable goods
   */

    class Program
    {
        private const string Ship1Data = @"c:\ship1_data.txt";
        private const string Ship2Data = @"c:\ship2_data.txt";

        private const int MaxLoad = 4;

        private static string[] ship1Load = new string[3];
        private static string[] ship2Load = new string[2];



        static void Main(string[] args)
        {
            LoadDate();
            Console.WriteLine("Current load:");
            PrintLoad();

        again:
            // data entry
            Console.Write("Load:");
            var load = Console.ReadLine();

            // logic

            if (load == "")
                return;

            if (!ContainerFound(ship1Load, load))
            {
                Console.WriteLine("Load " + load + " was not found on ship 1!");
                goto again;
            }

            TransferContainer(ref ship1Load, ref ship2Load, load);

            // Ausgabe
            Console.WriteLine(load + " was transfered from ship 1 to ship 2!");
            PrintLoad();

            goto again;
        }

        private static void LoadDate()
        {
            if (!File.Exists(Ship1Data))
            {
                // Data
                string containerA = "Container A";
                string containerB = "Container B";
                string containerC = "Container C";

                string containerD = "Container D";
                string containerE = "Container E";

                ship1Load = new string[] { containerA, containerB, containerC };
                ship2Load = new string[] { containerD, containerE };

            }
            else
            {
                LoadData(ref ship1Load, Ship1Data);
                LoadData(ref ship2Load, Ship2Data);
            }
           
        }

        private static void LoadData(ref string[] ship, string filePath)
        {
            try
            {
                var list = new List<string>();

                using (var sr = new StreamReader(filePath))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }

                ship = list.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private static bool ContainerFound(string[] ship, string container)
        {
            foreach (var item in ship)
            {
                if (item.Equals(container)) return true;
            }

            return false;
        }

        private static void PrintLoad()
        {
            Console.WriteLine("Ship 1: " + String.Join(", ", ship1Load));
            Console.WriteLine("Ship 2: " + String.Join(", ", ship2Load));
        }

        public static void TransferContainer(ref string[] shipFrom, ref string[] shipTo, string container)
        {

            if (shipTo.Length >= MaxLoad) return;

            var shipFromList = new List<string>();

            var shipToList = shipTo.ToList();

            foreach (var load in shipFrom)
            {
                if (load.Equals(container))
                {
                    shipToList.Add(load);
                }
                else
                {
                    shipFromList.Add(load);
                }

            }
            

            shipFrom = shipFromList.ToArray();
            shipTo = shipToList.ToArray();

            SaveData(shipFrom, Ship1Data);
            SaveData(shipTo, Ship2Data);
        }

        private static void SaveData(string[] ship, string filePath)
        {
            try
            {
                using (var sw = new StreamWriter(filePath))
                {
                    foreach (var container in ship)
                    {
                        sw.WriteLine(container);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
    }
}
