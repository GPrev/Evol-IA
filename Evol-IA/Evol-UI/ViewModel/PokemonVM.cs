using PokeMath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Type = PokeMath.Type;

namespace Evol_UI
{
    public class PokemonVM : Pokemon, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region New Fields
        public new int CurrHP
        {
            get
            {
                return base.CurrHP;
            }
            set
            {
                base.CurrHP = value;
                NotifyPropertyChanged("CurrHP");
            }
        }

        public new Condition Condition
        {
            get
            {
                return base.Condition;
            }
            set
            {
                base.Condition = value;
                NotifyPropertyChanged("Condition");
                NotifyPropertyChanged("ConditionStr");
            }
        }

        public string ConditionStr
        {
            get
            {
                switch(Condition)
                {
                    case Condition.BURNED:
                        return "BRN";
                    case Condition.FROZEN:
                        return "FRZ";
                    case Condition.PARALYZED:
                        return "PAR";
                    case Condition.POISONED:
                        return "PSN";
                    case Condition.ASLEEP:
                        return "SLP";
                    case Condition.FAINTED:
                        return "FNT";
                    default:
                        return "";
                }
            }
        }
        #endregion

        public PokemonVM(String name, int level, Type type, Type type2, int hP, int attack, int spAttack, int defense, int spDefense, int speed, List<Move> moves) :
            base(name, level, type, type2, hP, attack, spAttack, defense, spDefense, speed, moves)
        {}

        private void NotifyPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
