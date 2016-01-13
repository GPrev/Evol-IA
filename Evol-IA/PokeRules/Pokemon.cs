using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public enum Condition { OK, BURNED, FROZEN, PARALYZED, POISONED, ASLEEP, FAINTED }

    [Serializable]
    public class Pokemon : ICloneable
    {
        private PokeData data;

        #region Proxys
        public string Name { get { return data.Name; } }

        public int Level { get { return data.Level; } }
        public int HP { get { return data.HP; } }
        public int Attack { get { return data.Attack; } }
        public int SpAttack { get { return data.SpAttack; } }
        public int Defense { get { return data.Defense; } }
        public int SpDefense { get { return data.SpDefense; } }
        public int Speed { get { return data.Speed; } }

        public Type Type { get { return data.Type; } }
        public Type Type2 { get { return data.Type2; } }

        public List<Move> Moves { get { return data.Moves; } }
        #endregion

        private int currHP;
        public virtual int CurrHP
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

        public bool Ko()
        {
            return CurrHP == 0;
        }

        public virtual Condition Condition { get; set; }
        
        public Pokemon()
        {
            FullHeal();
        }

        public Pokemon(String name, int level, Type type, Type type2, int hP, int attack, int spAttack, int defense, int spDefense, int speed, List<Move> moves)
        {
            data = new PokeData(name, level, type, type2, hP, attack, spAttack, defense, spDefense, speed, moves);

            FullHeal();
        }

        public Pokemon(PokeData data)
        {
            this.data = data;
            FullHeal();
        }

        public void FullHeal()
        {
            this.CurrHP = HP;
            this.Condition = Condition.OK;
        }

        public object Clone()
        {
            return new Pokemon(data)
            { CurrHP = this.CurrHP, Condition = this.Condition };
        }
    }
}
