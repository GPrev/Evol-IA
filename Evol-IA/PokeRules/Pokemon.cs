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
        public int Attack { get { return (int)((float)data.Attack * GetStatMuliplier(Statistic.ATTACK)); } }
        public int SpAttack { get { return (int)((float)data.SpAttack * GetStatMuliplier(Statistic.SPATTACK)); } }
        public int Defense { get { return (int)((float)data.Defense * GetStatMuliplier(Statistic.DEFENSE)); } }
        public int SpDefense { get { return (int)((float)data.SpDefense * GetStatMuliplier(Statistic.SPDEFENSE)); } }
        public int Speed { get { return (int)((float)data.Speed * GetStatMuliplier(Statistic.SPEED)); } }

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

        private int statModifiers = 6 + 6*13 + 6*13*13 + 6*13*13*13 + 6*13*13*13*13;

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

        public void SetStatModifier(Statistic stat, int val)
        {
            if(val >= -6 && val <= 6 && stat != Statistic.NONE)
            {
                int oldVal = GetStatModifier(stat);
                AddStatModifier(stat, val - oldVal);
            }
        }

        public void AddStatModifier(Statistic stat, int val)
        {
            if (stat != Statistic.NONE)
            {
                int newVal = GetStatModifier(stat) + val;

                if (newVal > 6)
                    newVal = 6;
                else if (newVal < -6)
                    newVal = -6;

                int statID = (int)stat - 1; //-1 because HP doesn't change

                statModifiers += val * (int)Math.Pow(13, statID);
            }
        }

        public int GetStatModifier(Statistic stat)
        {
            if (stat == Statistic.NONE)
                return 0;
            //else
            int statID = (int)stat - 1; //-1 because HP doesn't change

            int res = ((statModifiers / (int)Math.Pow(13, statID)) % 13) - 6;
            return res;
        }

        public float GetStatMuliplier(Statistic stat)
        {
            int modifier = GetStatModifier(stat);

            if (modifier > 0)
                return (float)(2 + modifier) / 2;
            //else
            if (modifier < 0)
                return (float)2 / (2 - modifier);
            //else (= 0)
            return 1;
        }

        public void ResetStatModifiers()
        {
            for(int i = (int)Statistic.ATTACK; i < (int)Statistic.NONE; ++i)
            {
                SetStatModifier((Statistic)i, 0);
            }
        }

        public void FullHeal()
        {
            this.CurrHP = HP;
            this.Condition = Condition.OK;
        }

        public bool HasType(Type type)
        {
            return data.HasType(type);
        }

        public object Clone()
        {
            return new Pokemon(data)
            { CurrHP = this.CurrHP, Condition = this.Condition, statModifiers = this.statModifiers };
        }
    }
}
