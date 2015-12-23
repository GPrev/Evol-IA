using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattle
{
    public interface Intelligence
    {
        BattleAction ChooseAction(BattleState s, int myId = 1, ActionType type = ActionType.ANY);

        Trainer Trainer { get; }
    }
}
