using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Console;


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
        private const string filePath = @"c:\fleet.xml";

        private const int MaxLoad = 4;

        private static List<Ship> _fleet = new List<Ship>();



        static void Main(string[] args)
        {
            if (File.Exists(filePath))
            {
                _fleet = Load<List<Ship>>(filePath);

            }
            else
            {
                SeedData();
            }

            int key;
        menu:
            do
            {
                WriteLine("\nTo Create truck press 0 ship press 1 to Transfer load press 2");
                key = ReadKey().KeyChar;
                WriteLine(key);
            } while (key != 48 && key != 49 && key != 50);

            if (key == 49 || key == 48)
            {
                string name;
                do
                {
                    WriteLine("\nPlease enter ship or truck name:");
                    name = ReadLine();
                } while (string.IsNullOrEmpty(name));


                int capacity;
                do
                {
                    WriteLine("\nPlease enter capacity:");
                    capacity = Convert.ToInt32(ReadLine());
                } while (capacity <= 0);


                Ship vehicle;

                if (key == 48) vehicle = new Truck(name);
                else vehicle = new Ship(name);

                vehicle.Capacity = capacity;

                addContainer:
                WriteLine("\nTo Add conttainer press 1 to continue press 2");
                var choice = ReadKey().KeyChar;

                if (choice == 49)
                {
                    WriteLine("\nAdd container");
                    vehicle.Containers.Add(ReadLine());
                    if (vehicle.Containers.Count < vehicle.Capacity)
                        goto addContainer;

                }

                if (vehicle.Containers.Count < vehicle.Capacity && choice == 44) goto addContainer;


                _fleet.Add(vehicle);

                Save(_fleet, filePath);

                goto menu;



            }

            Console.WriteLine("Current load:");
            PrintLoad();

        chooseShipFrom:
            WriteLine("\nEnter the name ship from");
            string shipFromName = ReadLine();
            var shipFrom = _fleet.FirstOrDefault(s => s.Name.Equals(shipFromName));
            if (shipFrom == null) goto chooseShipFrom;

            chooseLoad:
            WriteLine("\nEnter the name of the load");
            var load = ReadLine();

            if (load == "" || !shipFrom.Containers.Any(c => c.Equals(load))) goto chooseLoad;

            chooseShipTo:
            WriteLine("\nEnter the name ship to");
            string shipToName = ReadLine();
            var shipTo = _fleet.FirstOrDefault(s => s.Name.Equals(shipToName));
            if (shipTo == null) goto chooseShipTo;


            WriteLine(load + $" was transfered from {shipFromName} to {shipToName}!");

            TransferContainer(shipFrom, shipTo, load);

            Save(_fleet, filePath);

            PrintLoad();

            goto chooseShipFrom;
        }

        private static void SeedData()
        {
            var ship1Load = new Ship("ship1Load");
            ship1Load.Capacity = 4;
            ship1Load.Containers = new List<string>()
            {
                "container A", "container B", "container C"
            };
            var ship2Load = new Ship("ship2Load");
            ship2Load.Capacity = 4;
            ship2Load.Containers = new List<string>()
            {
                "container D", "container E"
            };

            _fleet.Add(ship1Load);
            _fleet.Add(ship2Load);

        }


        private static void PrintLoad()
        {
            foreach (var ship in _fleet)
            {
                WriteLine("\n");
                WriteLine($"{ship.Name}: {String.Join(", ", ship.Containers.ToArray())}");
            }
        }

        public static void TransferContainer(Ship shipFrom, Ship shipTo, string container)
        {

            if (shipTo.Containers.Count >= shipTo.Capacity) return;
            if (shipFrom.GetType() == typeof(Truck) && shipFrom.Containers.Count == 1) return;

            shipFrom.Containers.Remove(container);
            shipTo.Containers.Add(container);

        }

        public static bool Save(Object obj, string filePath)
        {
            var xs = new XmlSerializer(obj.GetType());

            using (var sw = new StreamWriter(filePath))
            {

                xs.Serialize(sw, obj);
            }

            if (File.Exists(filePath))
                return true;
            else return false;
        }

        public static T Load<T>(string filePath)
        {
            Object result;

            if (File.Exists(filePath))
            {
                var xs = new XmlSerializer(typeof(T));

                using (var sr = new StreamReader(filePath))
                {
                    result = (T)xs.Deserialize(sr);
                }
                return (T)result;
            }

            return default(T);
        }
    }
}
