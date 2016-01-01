using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeRules;
using PokeRules;

namespace Evol_IA
{
    public class DumbAI : BattleAI
    {
        public Trainer trainer;

        public override Trainer Trainer
        {
            get { return trainer; }
        }

        public DumbAI(Trainer t = null)
        {
            if (t == null)
                trainer = new Trainer("DumbAI AI", new List<Pokemon>());
            else
                trainer = t;
        }

        public override BattleAction ChooseAction(BattleState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            List<BattleAction> actions = s.GetNextActions(myId);
            if (actions.Count > 0)
                return actions[0];
            else
                return null;
        }

        public override Trainer MakeTeam(List<Pokemon> availablePokemon, bool allowDoubles = false, int nbPokemon = 3)
        {
            if (availablePokemon.Count >= nbPokemon)
            {
                trainer.Team.Clear();
                for (int i = 0; i < nbPokemon; ++i)
                    trainer.Team.Add(availablePokemon[i]);

                trainer.ActivePokemon = trainer.Team[0];
            }
            return Trainer;
        }
    }
}
