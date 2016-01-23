using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public class Battle
    {
        public OutDel outDel = message => { return; };

        List<Trainer> Trainers { get; set; }
        List<Intelligence> Intelligences { get; set; }

        BattleState state;

        public Battle(List<Intelligence> intelligences, OutDel del = null)
        {
            Intelligences = intelligences;

            Trainers = new List<Trainer>();
            foreach (Intelligence i in Intelligences)
                Trainers.Add(i.Trainer);

            if (del != null)
                outDel = del;

            state = new BattleState(Trainers, outDel);
        }

        public int PlayBattle()
        {
            while (!state.HasWinner())
            {
                List<BattleAction> actions = new List<BattleAction>();
                List<ActionType> expected = state.NextActionTypes;
                for(int i = 0; i < Trainers.Count; ++i)
                {
                    if (expected[i] == ActionType.NONE)
                        actions.Add(null);
                    else
                        actions.Add(Intelligences[i].ChooseAction(state, i, expected[i]));
                }
                state.MakeActions(actions);
            }
            return state.Winner();
        }
    }
}
