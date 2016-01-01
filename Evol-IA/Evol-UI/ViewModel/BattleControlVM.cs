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
    public class BattleControlVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BattleVM Battle { get; private set; }

        private int id;

        public Trainer Ally { get { return Battle.Trainers[id]; } }
        public Trainer Oponent { get { return Battle.Trainers[1-id]; } } // Only for 1v1

        public bool CanAct { get { return CanMakeMove || CanSwitch; } }
        
        public bool CanMakeMove { get { return PossibleMoves.Count > 0; } }

        public bool CanSwitch { get { return PossibleSwitches.Count > 0; } }

        public List<Move> PossibleMoves { get; set; }
        public List<Pokemon> PossibleSwitches { get; set; }

        public Move PendingMove
        {
            get
            {
                BattleAction a = Battle.GetPendingAction(id);
                if (a == null)
                    return null;
                //else
                return a.GetMove();
            }
            set
            {
                Battle.SeletAction(id, value);
                //if(Battle.SeletAction(id, value))
                //{
                    NotifyPropertyChanged("PendingMove");
                    NotifyPropertyChanged("PendingSwitch");
                //}
            }
        }

        public Pokemon PendingSwitch
        {
            get {
                BattleAction a = Battle.GetPendingAction(id);
                if (a == null)
                    return null;
                //else
                return a.GetPokemon();
            }
            set
            {
                Battle.SeletAction(id, value);
                //if (Battle.SeletAction(id, value))
                //{
                    NotifyPropertyChanged("PendingMove");
                    NotifyPropertyChanged("PendingSwitch");
                //}
            }
        }

        public BattleControlVM(BattleVM battle, int tID)
        {
            this.Battle = battle;
            this.id = tID;

            PossibleMoves = new List<Move>();
            PossibleSwitches = new List<Pokemon>();
            RefreshPossibleLists();
        }

        private void NotifyPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void RefreshPossibleLists()
        {
            List<BattleAction> moves = Battle.GetPossibleMoves(id);
            PossibleMoves.Clear();
            foreach (BattleAction a in moves)
                PossibleMoves.Add(a.GetMove());

            List<BattleAction> switches = Battle.GetPossiblePokemon(id);
            PossibleSwitches.Clear();
            foreach (BattleAction a in switches)
                PossibleSwitches.Add(a.GetPokemon());

            NotifyPropertyChanged("PossibleMoves");
            NotifyPropertyChanged("PossibleSwitches");
            NotifyPropertyChanged("CanMakeMove");
            NotifyPropertyChanged("CanSwitch");
            NotifyPropertyChanged("CanAct");
        }
    }
}
