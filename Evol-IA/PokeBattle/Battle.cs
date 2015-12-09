using PokeMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattle
{
    public class Battle
    {
        static readonly Rules rules = new G4Rules();

        public delegate void OutDel(string str);

        public OutDel outDel = message => { Console.WriteLine(message); };

        List<Trainer> Trainers { get; set; }

        public Battle(List<Trainer> trainers, OutDel del = null)
        {
            Trainers = trainers;
            if (del != null)
                outDel = del;
        }

        public void PlayTurn()
        {
            List<BattleAction> actions = new List<BattleAction>();
            List<Pokemon> attP = new List<Pokemon>();
            List<Move> moves = new List<Move>();
            foreach (Trainer t in Trainers)
            {
                BattleAction a = t.ChooseAction();
                actions.Add(a);

                // Pokemon switches
                if (a.ActionType == ActionType.POKEMON)
                {
                    switchPokemon(t, a.GetMoveOrPokemon().Item2);
                }
                else if (a.ActionType == ActionType.FIGHT)
                {
                    attP.Add(t.ActivePokemon);
                    moves.Add(a.GetMoveOrPokemon().Item1);
                }
            }

            // Attacks are now
            List<int> priority = rules.OrderMoves(attP, moves);
            for (int i = 0; i < priority.Count; ++i)
            {
                int p = priority[i];
                Move m = moves[p];
                Pokemon attacker = attP[p];
                Pokemon defender = attP[1 - p]; // Only works for 2 pokemon

                if (!attacker.Ko())
                {
                    outDel(attacker.Name + " uses " + m.Name + ".");
                    defender.CurrHP -= rules.DamageFormula(attacker, defender, m);

                    float effective = rules.GetTypeModifier(m, attacker);
                    if (effective > 1)
                        outDel("It's super effective !");
                    else if (effective < 1)
                        outDel("It's not very effective...");

                    if (defender.Ko())
                        outDel(defender.Name + " fainted !");
                }
            }

            // Resolve KOs
            foreach (Trainer t in Trainers)
            {
                if (t.IsOutOfPokemon())
                    return;
            }
            foreach (Trainer t in Trainers)
            {
                if (t.ActivePokemon.Ko())
                {
                    PokemonAction a = t.ChoosePokemon();
                    switchPokemon(t, a.Pokemon);
                }
            }
        }

        private void switchPokemon(Trainer t, Pokemon pokemon)
        {
            t.ActivePokemon = pokemon;
            outDel(t.Name + " sends out " + pokemon.Name + " !");
        }

        public Trainer PlayBattle()
        {
            outDel(Trainers[0].Name + " and " + Trainers[1].Name + " want to fight !");
            while (Winner() == null)
            {
                PlayTurn();
            }
            outDel(Winner().Name + " has lost !");
            return Winner();
        }

        Trainer Winner()
        {
            if (Trainers[0].IsOutOfPokemon())
            {
                return Trainers[1];
            }
            //else
            if (Trainers[1].IsOutOfPokemon())
            {
                return Trainers[0];
            }
            //else
            return null;
        }
    }
}
