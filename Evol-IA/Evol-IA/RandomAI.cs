using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeBattle;
using PokeMath;

namespace Evol_IA
{
    public class RandomAI : BattleAI
    {
        Random myRand = new Random();

        public Trainer trainer;

        public override Trainer Trainer
        {
            get { return Trainer; }
        }

        public RandomAI(Trainer t = null)
        {
            if (t == null)
                trainer = new Trainer("Random AI", new List<Pokemon>());
            else
                trainer = t;
        }

        public override BattleAction ChooseAction(BattleState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            List<BattleAction> actions = s.GetNextActions(myId);
            if (actions.Count > 0)
                return actions[myRand.Next(actions.Count)];
            else
                return null;
        }


        public override Trainer MakeTeam(List<Pokemon> availablePokemon, bool allowDoubles = false, int nbPokemon = 3)
        {
            if (availablePokemon.Count >= nbPokemon)
            {
                trainer.Team.Clear();
                for (int i = 0; i < nbPokemon; ++i)
                {
                    Pokemon p;
                    do
                    {
                        p = availablePokemon[myRand.Next(availablePokemon.Count)];
                    } while (!allowDoubles && trainer.Team.Contains(p));

                    trainer.Team.Add(p);
                }
                trainer.ActivePokemon = trainer.Team[0];
            }
            return Trainer;
        }
    }
}
