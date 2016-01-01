using PokeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public class Battle
    {
        public static readonly Rules rules = new G4Rules();

        public delegate void OutDel(string str);

        public OutDel outDel = message => { Console.WriteLine(message); };

        List<Trainer> Trainers { get; set; }
        List<Intelligence> Intelligences { get; set; }

        BattleState state;

        public Battle(List<Intelligence> intelligences, OutDel del = null)
        {
            Intelligences = intelligences;

            Trainers = new List<Trainer>();
            foreach (Intelligence i in Intelligences)
                Trainers.Add(i.Trainer);

            state = new BattleState(Trainers);

            if (del != null)
                outDel = del;
        }

        public Trainer PlayBattle()
        {
            while (state.Winner() < 0)
            {
                List<BattleAction> actions = new List<BattleAction>();
                List<ActionType> expected = state.NextActionTypes;
                for(int i = 0; i < Trainers.Count; ++i)
                {
                    if (expected[i] == ActionType.NONE)
                        actions.Add(null);
                    else
                        Intelligences[i].ChooseAction(state, i, expected[i]);
                }
                state.MakeActions(actions);
            }
            return Trainers[state.Winner()];
        }
    }
}
