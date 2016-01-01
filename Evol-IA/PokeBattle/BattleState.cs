using PokeMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattle
{
    public class BattleState
    {
        Rules rules = Battle.rules;

        public List<Trainer> Trainers { get; protected set; }

        public List<ActionType> NextActionTypes { get; private set; }

        public BattleState(List<Trainer> trainers)
        {
            Trainers = trainers;
            NextActionTypes = new List<ActionType>();
            foreach (Trainer t in Trainers)
                NextActionTypes.Add(ActionType.ANY);
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
            foreach (Move m in Trainers[trainerId].ActivePokemon.Moves)
            {
                //1-id only works when 2 trainers
                res.Add(new FightAction(Trainers[trainerId].ActivePokemon, Trainers[1-trainerId].ActivePokemon, m));
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
            List<Pokemon> attP = new List<Pokemon>();
            List<Pokemon> defP = new List<Pokemon>();
            List<Move> moves = new List<Move>();
            for (int i = 0; i < Trainers.Count; ++i)
            {
                BattleAction a = actions[i];
                // Pokemon switches
                if (a.GetActionType() == ActionType.POKEMON)
                {
                    Trainers[i].ActivePokemon = a.GetMoveOrPokemon().Item2;
                }
                else if (a.GetActionType() == ActionType.FIGHT)
                {
                    attP.Add(Trainers[i].ActivePokemon);
                    defP.Add(Trainers[1-i].ActivePokemon); // Only works for 2 pokemon
                    moves.Add(a.GetMove());
                }
            }

            // Attacks are now
            List<int> priority = rules.OrderMoves(attP, moves);
            for (int i = 0; i < priority.Count; ++i)
            {
                int p = priority[i];
                Move m = moves[p];
                Pokemon attacker = attP[p];
                Pokemon defender = defP[p];

                if (!attacker.Ko())
                {
                    defender.CurrHP -= rules.DamageFormula(attacker, defender, m);
                }
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
                for (int i = 0; i < Trainers.Count; ++i)
                {
                    NextActionTypes[i] = ActionType.ANY;
                }
            }
        }

        bool HasWinner()
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
    }
}
