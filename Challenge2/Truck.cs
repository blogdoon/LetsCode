using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge2
{
    [Serializable]
    public class Truck : Ship
    {
        public Truck()
        {

        }

        public Truck(string name) : base(name)
        {

        }
    }
}
