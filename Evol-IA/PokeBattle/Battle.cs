using PokeMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattle
{
    class Battle
    {
        static readonly Rules rules = new G4Rules();

        List<Trainer> Trainers { get; set; }

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
                    t.ActivePokemon = a.Pokemon;
                }
            }
        }

        public Trainer PlayBattle()
        {
            while (Winner() == null)
            {
                PlayTurn();
            }
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
