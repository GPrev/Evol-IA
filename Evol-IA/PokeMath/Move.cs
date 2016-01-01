using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    [Serializable]
    public class Move
    {
        public int Power { get; set; }
        public int Accuracy { get; set; }
        public bool Special { get; set; }

        public Type Type { get; set; }
        public string Name { get; set; }

        public Move() { }

        public Move(string name, Type type, int power, int acc, bool spe)
        {
            Name = name;
            Type = type;
            Power = power;
            Accuracy = acc;
            Special = spe;
        }
    }
}
