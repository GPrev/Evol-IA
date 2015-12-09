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
            if(del != null)
                outDel = del;
        }

        public void PlayTurn()
        {
            List<BattleAction> actions = new List<BattleAction>();
            foreach (Trainer t in Trainers)
            {
                BattleAction a = t.ChooseAction();
                actions.Add(a);

                // Pokemon switches
                if (a.ActionType == ActionType.POKEMON)
                {
                    t.ActivePokemon = a.GetMoveOrPokemon().Item2;
                }
            }

            rules.resolveTurn(Trainers[0].ActivePokemon, Trainers[1].ActivePokemon,
                actions[0].GetMoveOrPokemon().Item1, actions[1].GetMoveOrPokemon().Item1);

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
            outDel(t.Name + " sends " + pokemon.Name + " !");
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
