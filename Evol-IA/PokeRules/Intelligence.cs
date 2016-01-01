using PokeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public interface Intelligence
    {
        BattleAction ChooseAction(BattleState s, int myId = 1, ActionType type = ActionType.ANY);

        Trainer Trainer { get; }
    }
}
