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
            {
                MctsNode n2 = new MctsNode(s2, this, a);
                this.Children.Add(n2);
                return n2;
            }
            //else
            return null;
        }

        public override string ToString()
        {
            string res = "";
            if (Action == null)
                res += "Initial State";
            else if (Action.GetActionType() == ActionType.FIGHT)
                res += "Fight - " + (Action as FightAction).Move.Name;
            else
                res += "Pokemon - " + (Action as PokemonAction).getPokemon(State.State).Name;

            res += "\\n (" + GetHashCode() + " )";

            res += "\\n Val=" + Value + " Vis=" + Visits;

            return res;
        }

        public string ToGrapVizString(int level = 0)
        {
            string res = "";

            if (level == 0)
                res += "digraph g{" + System.Environment.NewLine;

            foreach (MctsNode c in Children)
            {
                res += "\"(" + level + ") " + ToString() + "\" -> \"(" + (level+1) + ") " + c.ToString() + "\"" + System.Environment.NewLine;
            }

            foreach (MctsNode c in Children)
            {
                res += c.ToGrapVizString(level + 1);
            }

            if (level == 0)
                res += "}";

            return res;
        }
    }
}
