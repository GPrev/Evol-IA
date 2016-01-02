﻿using PokeRules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evol_IA;

namespace Evol_UI
{
    public class BattleVM : BattleState, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<BattleAction> PendingActions;

        public ObservableCollection<BattleControlVM> BattleControls { get; private set; }

        private List<BattleAI> AIs;

        public String Log { get; private set; } = "";

        // Give as many ais as trainers (set some to null if necessary)
        public BattleVM(List<BattleAI> ais, List<Trainer> trainers) : base(ExtractTeams(ais, trainers))
        {
            this.AIs = ais;
            this.outD = message => { AppendLog(message); };
            DisplayInitMessage();
            Init();
        }

        public BattleVM(List<Trainer> trainers) : base(trainers)
        {
            Init();
        }

        private static List<Trainer> ExtractTeams(List<BattleAI> ais, List<Trainer> trainers = null)
        {
            List<Trainer> res = new List<Trainer>();
            for (int i = 0; i < ais.Count; ++i)
            {
                if (ais[i] != null)
                    res.Add(ais[i].Trainer);
                else if (trainers != null)
                    res.Add(trainers[i]);
                else // Should not happen
                    res.Add(null);
            }
            return res;
        }

        private void Init()
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

            // AIs first move
            MakeAIChoice();
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
            Trainer defender = Trainers[1-tID]; // Only works in 1v1
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

                    // Makes AI decisions
                    MakeAIChoice();

                    NotifyPropertyChanged("BattleState");
                }
            }
            return res;
        }

        private void ResetPending()
        {
            for (int i = 0; i < PendingActions.Count; ++i)
                PendingActions[i] = null;
            for (int i = PendingActions.Count; i < Trainers.Count; ++i)
                PendingActions.Add(null);
            NotifyPropertyChanged("PendingActions");
        }

        private void MakeAIChoice()
        {
            if (AIs != null)
            {
                for (int i = 0; i < AIs.Count; ++i)
                {
                    if (AIs[i] != null && NextActionTypes[i] != ActionType.NONE)
                    {
                        BattleAction choice = AIs[i].ChooseAction(this, i, NextActionTypes[i]);
                        SeletAction(i, choice);
                    }
                }
            }
        }

        private void AppendLog(string message)
        {
            if (Log.Length > 0)
                Log += System.Environment.NewLine;
            Log += message;

            NotifyPropertyChanged("Log");

            Console.WriteLine(message);
        }
    }
}
