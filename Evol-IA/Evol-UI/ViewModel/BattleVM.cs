using PokeBattle;
using PokeMath;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evol_UI
{
    public class BattleVM : BattleState, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<BattleAction> PendingActions;

        public ObservableCollection<BattleControlVM> BattleControls { get; private set; }

        public BattleVM(List<Trainer> trainers) : base(trainers)
        {
            PendingActions = new List<BattleAction>();
            BattleControls = new ObservableCollection<BattleControlVM>();
            for (int i = 0; i < Trainers.Count; ++i)
            {
                BattleControls.Add(new BattleControlVM(this, i));
                PendingActions.Add(null);
            }

            NotifyPropertyChanged("PendingActions");
            NotifyPropertyChanged("BattleControls");
        }

        private void NotifyPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public BattleAction GetPendingAction(int tID) { return PendingActions[tID]; }

        public bool SeletAction(int tID, Move m)
        {
            Pokemon attacker = Trainers[tID].ActivePokemon;
            Pokemon defender = Trainers[1-tID].ActivePokemon; // Only works in 1v1
            return SeletAction(tID, new FightAction(attacker, defender, m));
        }

        public bool SeletAction(int tID, Pokemon p)
        {
            return SeletAction(tID, new PokemonAction(Trainers[tID], p));
        }

        public bool SeletAction(int tID, BattleAction action)
        {
            bool res = GetNextActions(tID).Contains(action);
            if(res)
            {
                PendingActions[tID] = action;
                if(CanMakeActions(PendingActions))
                {
                    MakeActions(PendingActions);
                    ResetPending();
                }
            }
            return res;
        }

        private void ResetPending()
        {
            for (int i = 0; i < PendingActions.Count; ++i)
                PendingActions[i] = null;
        }
    }
}
