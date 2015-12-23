using PokeBattle;
using PokeMath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evol_UI
{
    public class TrainerVM : Trainer, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region New Fields
        public new Pokemon ActivePokemon
        {
            get { return base.ActivePokemon; }
            set
            {
                base.ActivePokemon = value;
                NotifyPropertyChanged("ActivePokemon");
                NotifyPropertyChanged("ActivePokemonVM");
            }
        }

        public PokemonVM ActivePokemonVM
        {
            get { return ActivePokemon as PokemonVM; }
            set
            {
                ActivePokemon = value;
                NotifyPropertyChanged("ActivePokemon");
                NotifyPropertyChanged("ActivePokemonVM");
            }
        }
        #endregion

        public TrainerVM(string name, List<Pokemon> team) : base(name, team)
        { }

        private void NotifyPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
