﻿using PokeRules;
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
        public Trainer Oponent { get { return Battle.Trainers[1 - id]; } } // Only for 1v1

        public bool CanAct { get { return CanFight || CanSwitch; } }

        public bool CanFight { get { return PossibleMoves.Count > 0 && Battle.GetPendingAction(id) == null; } }

        public bool CanSwitch { get { return PossibleSwitches.Count > 0 && Battle.GetPendingAction(id) == null; } }

        public List<Move> PossibleMoves { get; set; }
        public List<Pokemon> PossibleSwitches { get; set; }

        public Move PendingMove
        {
            get
            {
                FightAction a = Battle.GetPendingAction(id) as FightAction;
                if (a == null)
                    return null;
                //else
                return a.Move;
            }
            set
            {
                Battle.SeletAction(id, value);

                NotifyPropertyChanged("PendingMove");
                NotifyPropertyChanged("PendingSwitch");
                NotifyPropertyChanged("CanFight");
                NotifyPropertyChanged("CanSwitch");
            }
        }

        public Pokemon PendingSwitch
        {
            get {
                PokemonAction a = Battle.GetPendingAction(id) as PokemonAction;
                if (a == null)
                    return null;
                //else
                return a.getPokemon(Battle);
            }
            set
            {
                Battle.SeletAction(id, value);

                NotifyPropertyChanged("PendingMove");
                NotifyPropertyChanged("PendingSwitch");
                NotifyPropertyChanged("CanFight");
                NotifyPropertyChanged("CanSwitch");
            }
        }

        public BattleControlVM(BattleVM battle, int tID)
        {
            this.Battle = battle;
            this.id = tID;

            Battle.PropertyChanged += OnBattlePropertyChanged;

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
            PossibleMoves.Clear();
            PossibleSwitches.Clear();

            if (Battle.NextActionTypes[id] == ActionType.FIGHT
                || Battle.NextActionTypes[id] == ActionType.ANY)
            {
                List<BattleAction> moves = Battle.GetPossibleMoves(id);
                foreach (BattleAction a in moves)
                {
                    FightAction aa = a as FightAction;
                    if(aa != null)
                        PossibleMoves.Add(aa.Move);
                }
            }

            if (Battle.NextActionTypes[id] == ActionType.POKEMON
                || Battle.NextActionTypes[id] == ActionType.ANY)
            {
                List<BattleAction> switches = Battle.GetPossiblePokemon(id);
                foreach (BattleAction a in switches)
                {
                    PokemonAction aa = a as PokemonAction;
                    if (aa != null)
                        PossibleSwitches.Add(aa.getPokemon(Battle));
                }
            }

            NotifyPropertyChanged("PossibleMoves");
            NotifyPropertyChanged("PossibleSwitches");
            NotifyPropertyChanged("CanFight");
            NotifyPropertyChanged("CanSwitch");
            NotifyPropertyChanged("CanAct");
        }

        void OnBattlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PendingActions":
                    NotifyPropertyChanged("PendingMove");
                    NotifyPropertyChanged("PendingSwitch");
                    NotifyPropertyChanged("CanFight");
                    NotifyPropertyChanged("CanSwitch");
                    break;
                case "BattleState":
                    RefreshPossibleLists();
                    break;
                default:
                    break;
            }
        }
    }
}
