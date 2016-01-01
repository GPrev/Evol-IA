using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeBattle;
using PokeMath;

namespace Evol_IA
{
    class RandomAI : BattleAI
    {
        Random myRand = new Random();

        public override Trainer Trainer
        {
            get { return new Trainer("Random AI", new List<Pokemon>()); }
        }

        public override BattleAction ChooseAction(BattleState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            List<BattleAction> actions = s.GetNextActions(myId);
            if (actions.Count > 0)
                return actions[myRand.Next(actions.Count)];
            else
                return null;
        }
    }
}
