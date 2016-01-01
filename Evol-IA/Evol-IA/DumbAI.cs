using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeBattle;
using PokeMath;

namespace Evol_IA
{
    class DumbAI : BattleAI
    {
        public override Trainer Trainer
        {
            get { return new Trainer("Dumb AI", new List<Pokemon>()); }
        }

        public override BattleAction ChooseAction(BattleState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            List<BattleAction> actions = s.GetNextActions(myId);
            if (actions.Count > 0)
                return actions[0];
            else
                return null;
        }
    }
}
