using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    [Serializable]
    public class Move
    {
        public string Name { get; set; }
        public Type Type { get; set; }

        public int Power { get; set; }
        public int Accuracy { get; set; }
        public bool Special { get; set; }

        [OptionalField]
        public Condition Condition = Condition.OK;

        public Move() { }

        public Move(string name, Type type, int power, int acc, bool spe, Condition cond = Condition.OK)
        {
            Name = name;
            Type = type;
            Power = power;
            Accuracy = acc;
            Special = spe;
            Condition = cond;
        }
    }
}
