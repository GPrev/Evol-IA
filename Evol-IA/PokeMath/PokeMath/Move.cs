using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    class Move
    {
        public int Power { get; set; }
        public int Accuracy { get; set; }
        public bool Special { get; set; }

        public Type Type { get; set; }
    }
}
