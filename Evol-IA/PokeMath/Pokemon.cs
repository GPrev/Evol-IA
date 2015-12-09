using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    public enum Condition { BURNED, FROZEN, PARALYZED, POISONED, ASLEEP, OK }
    public enum Type { NORMAL, FIGHT, FLYING, POISON, GROUND, ROCK, BUG, GHOST, STEEL, FIRE, WATER, ELECTRIC, PSYCHIC, ICE, DRAGON, DARK, FAIRY, NONE }
    public class Pokemon
    {
        String Name { get; set; }

        int currHP;
        int CurrHP
        {
            get { return currHP; }
            set {
                currHP = value;
                if (currHP < 0)
                    currHP = 0;
                if (currHP > HP)
                    currHP = HP;
            }
        }

        Condition Condition { get; set; }

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

        List<Move> Moves { get; set; }

        public Pokemon(String name, int level, Type type, Type type2, int hP, int attack, int spAttack, int defense, int spDefense, int speed)
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

            fullHeal();
        }

        public void fullHeal()
        {
            this.CurrHP = HP;
            this.Condition = Condition.OK;
        }
    }
}
