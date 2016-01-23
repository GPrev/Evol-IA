using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public enum Type { NORMAL, FIGHT, FLYING, POISON, GROUND, ROCK, BUG, GHOST, STEEL, FIRE, WATER, GRASS, ELECTRIC, PSYCHIC, ICE, DRAGON, DARK, FAIRY, NONE }

    [Serializable]
    public class PokeData
    {
        public string Name { get; set; }
        
        #region Stats
        public int Level { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int SpAttack { get; set; }
        public int Defense { get; set; }
        public int SpDefense { get; set; }
        public int Speed { get; set; }
        #endregion

        public Type Type { get; set; }
        public Type Type2 { get; set; }

        public List<Move> Moves { get; set; }

        public PokeData() { }

        public PokeData(String name, int level, Type type, Type type2, int hP, int attack, int spAttack, int defense, int spDefense, int speed, List<Move> moves)
        {
            this.Name = name;

            this.Type = type;
            this.Type2 = type2;

            this.Level = level;
            this.HP = hP;
            this.Attack = attack;
            this.SpAttack = spAttack;
            this.Defense = defense;
            this.SpDefense = spDefense;
            this.Speed = speed;

            this.Moves = moves;
        }

        public bool HasType(Type type)
        {
            return Type == type || Type2 == type;
        }
    }
}
