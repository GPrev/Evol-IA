using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    public class Move
    {
        public int Power { get; set; }
        public int Accuracy { get; set; }
        public bool Special { get; set; }

        public Type Type { get; set; }

        public Move(Type type, int power, int acc, bool spe)
        {
            Type = type;
            Power = power;
            Accuracy = acc;
            Special = spe;
        }
    }
}
