using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public class BattleState : ICloneable
    {
        public List<Trainer> Trainers { get; protected set; }

        public List<ActionType> NextActionTypes { get; private set; }

        bool makingMoves = false;

        protected OutDel outD = null;

        public BattleState(List<Trainer> trainers, OutDel outD = null)
        {
            Trainers = trainers;
            NextActionTypes = new List<ActionType>();
            foreach (Trainer t in Trainers)
                NextActionTypes.Add(ActionType.ANY);

            this.outD = outD;
        }
        

        protected void DisplayInitMessage()
        {
            outD(Trainers[0].Name + " and " + Trainers[1].Name + " want to fight !");
            outD(Trainers[0].Name + " sends out " + Trainers[0].ActivePokemon.Name + " !");
            outD(Trainers[1].Name + " sends out " + Trainers[1].ActivePokemon.Name + " !");
        }

        public List<BattleAction> GetNextActions(int trainerId)
        {
            List<BattleAction> res;
            switch(NextActionTypes[trainerId])
            {
                case ActionType.ANY:
                    res = GetPossibleMoves(trainerId);
                    res.AddRange(GetPossiblePokemon(trainerId));
                    break;
                case ActionType.FIGHT:
                    res = GetPossibleMoves(trainerId);
                    break;
                case ActionType.POKEMON:
                    res = GetPossiblePokemon(trainerId);
                    break;
                default: //NONE
                    res = new List<BattleAction>();
                    break;
            }
            return res;
        }

        public List<BattleAction> GetPossibleMoves(int trainerId)
        {
            List<BattleAction> res = new List<BattleAction>();
            if (Trainers[trainerId].ActivePokemon != null)
            {
                foreach (Move m in Trainers[trainerId].ActivePokemon.Moves)
                {
                    //1-id only works when 2 trainers
                    res.Add(new FightAction(Trainers[trainerId].ActivePokemon, Trainers[1 - trainerId], m));
                }
            }
            return res;
        }

        public List<BattleAction> GetPossiblePokemon(int trainerId)
        {
            List<BattleAction> res = new List<BattleAction>();
            foreach (Pokemon p in Trainers[trainerId].Team)
            {
                if (p != Trainers[trainerId].ActivePokemon && !p.Ko())
                    res.Add(new PokemonAction(Trainers[trainerId],  p));
            }
            return res;
        }

        public bool CanMakeActions(List<BattleAction> actions)
        {
            if (makingMoves)
                return false;

            for(int i = 0; i < Trainers.Count; ++i)
            {
                if (NextActionTypes[i] != ActionType.NONE && actions[i] != null)
                {
                    if (!GetNextActions(i).Contains(actions[i]))
                        return false;
                }
                else if (NextActionTypes[i] != ActionType.NONE || actions[i] != null)
                    return false;
            }
            return true;
        }

        public void MakeActions(List<BattleAction> actions)
        {
            makingMoves = true;
            Rules.ActiveRules.OrderActions(actions);
            for (int i = 0; i < actions.Count; ++i)
            {
                actions[i].SafeExecute(outD);
            }

            for (int i = 0; i < Trainers.Count; ++i)
            {
                NextActionTypes[i] = ActionType.NONE;
            }

            // Resolve KOs
            bool needsChange = false;
            for (int i = 0; i < Trainers.Count; ++i)
            {
                if (Trainers[i].ActivePokemon.Ko())
                {
                    if (Trainers[i].IsOutOfPokemon())
                    {
                        for (int j = 0; j < Trainers.Count; ++j)
                        {
                            NextActionTypes[j] = ActionType.NONE;
                        }
                        outD(Trainers[i].Name + " has lost !");
                        makingMoves = false;
                        return;
                    }
                    //else
                    needsChange = true;
                    NextActionTypes[i] = ActionType.POKEMON;
                }
            }

            if(!needsChange)
            {
                //Next turn
                outD("============================");
                for (int i = 0; i < Trainers.Count; ++i)
                {
                    NextActionTypes[i] = ActionType.ANY;
                }
            }
            makingMoves = false;
        }

        public bool HasWinner()
        {
            return Winner() >= 0;
        }

        public int Winner()
        {
            if (Trainers[0].IsOutOfPokemon())
            {
                return 1;
            }
            //else
            if (Trainers[1].IsOutOfPokemon())
            {
                return 0;
            }
            //else
            return -1;
        }

        public bool IsOutOfPokemon(List<Pokemon> team)
        {
            foreach (Pokemon p in team)
            {
                if (!p.Ko())
                    return false;
            }
            return true;
        }

        public object Clone()
        {
            // Clone Trainers
            List<Trainer> newTrainers = new List<Trainer>();
            foreach (Trainer t in Trainers)
                newTrainers.Add(t.Clone() as Trainer);

            // Clone ActionTypes
            List<ActionType> newNexts = new List<ActionType>();
            foreach (ActionType t in NextActionTypes)
                newNexts.Add(t);

            BattleState res = new BattleState(newTrainers) { NextActionTypes = newNexts };
            return res;
        }
    }
}
