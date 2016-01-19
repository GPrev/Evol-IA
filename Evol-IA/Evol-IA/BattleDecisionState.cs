using PokeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evol_IA
{
    public class BattleDecisionState
    {
        public BattleState State { get; private set; }
        List<BattleAction> pendingActions;

        public BattleDecisionState(BattleState s, List<BattleAction> pActions = null)
        {
            State = s;
            if(pActions == null)
            {
                pendingActions = new List<BattleAction>();
                for (int i = 0; i < State.Trainers.Count; ++i)
                {
                    pendingActions.Add(null);
                }
            }
            else
            {
                pendingActions = pActions;
            }
        }

        public BattleDecisionState GetChild(BattleAction a, int id)
        {
            if(a == null)
            {
                return new BattleDecisionState(State, pendingActions);
            }
            //else
            List<BattleAction> newActions = new List<BattleAction>();
            for (int i = 0; i < pendingActions.Count; ++i)
            {
                if(i == id)
                {
                    newActions.Add(a);
                }
                else
                {
                    newActions.Add(this.pendingActions[i]);
                }
            }
            BattleDecisionState res;
            if (State.CanMakeActions(newActions))
            {
                BattleState newState = State.Clone() as BattleState;
                newState.MakeActions(newActions);
                res = new BattleDecisionState(newState);
            }
            else
            {
                res = new BattleDecisionState(State, newActions);
            }
            return res;
        }

        public List<BattleDecisionState> GetChildren(int id)
        {
            List<BattleAction> actions = State.GetNextActions(id);
            List<BattleDecisionState> res = new List<BattleDecisionState>();

            foreach(BattleAction a in actions)
            {
                res.Add(GetChild(a, id));
            }
            // If no possible action, returns a clone
            if(res.Count == 0)
            {
                res.Add(new BattleDecisionState(State, pendingActions));
            }

            return res;
        }
    }
}
