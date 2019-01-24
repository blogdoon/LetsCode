using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge2
{
    [Serializable]
    public class Ship
    {

        public Ship()
        {

        }

        public Ship(string name)
        {
            Name = name;
            Containers = new List<string>();
        }
        public List<string> Containers { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}
