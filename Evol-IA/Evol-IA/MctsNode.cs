using PokeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evol_IA
{
    public class MctsNode
    {
        public BattleDecisionState State { get; private set; }
        public MctsNode Parent { get; private set; }
        public List<MctsNode> Children { get; private set; }
        public BattleAction Action { get; private set; }
        public double Value { get; set; }
        public int Visits { get; set; }

        public MctsNode(BattleDecisionState s, MctsNode parent = null, BattleAction a = null)
        {
            State = s;
            Action = a;
            Parent = parent;
            Children = new List<MctsNode>();
            Value = 0;
            Visits = 0;
        }

        public MctsNode MakeChild(BattleAction a, int id)
        {
            BattleDecisionState s2 = State.GetChild(a, id);
            if (s2 != null)
                return new MctsNode(s2, this, a);
            //else
            return null;
        }
    }
}
