using PokeRules;
using PokeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evol_IA
{
    public abstract class BattleAI : Intelligence
    {
        public abstract Trainer Trainer { get; }

        public abstract Trainer MakeTeam(List<Pokemon> availablePokemon, bool allowDoubles = false, int nbPokemon = 3);

        public abstract BattleAction ChooseAction(BattleState s, int myId = 1, ActionType type = ActionType.ANY);
    }
}
