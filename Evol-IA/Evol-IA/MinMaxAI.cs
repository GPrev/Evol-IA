using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeRules;

namespace Evol_IA
{
    public class MinMaxAI : BattleAI
    {
        public Trainer trainer;
        int maxprof;

        public override Trainer Trainer
        {
            get { return trainer; }
        }

        public MinMaxAI(Trainer t = null)
        {
            if (t == null)
                trainer = new Trainer("DumbAI AI", new List<Pokemon>());
            else
                trainer = t;

            maxprof = 5;
        }

        public override BattleAction ChooseAction(BattleDecisionState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            float max = -10000;
            float tmp;
            BattleAction act=null;

            List<BattleAction> actions = s.State.GetNextActions(myId);

            foreach (BattleAction a in actions)
            {

                BattleDecisionState sc = s.GetChild(a, myId);
                tmp = Min(sc, 0, myId);

                Console.WriteLine("Action " + a + " Score " + tmp);

                if (tmp> max)
                {
                    act = a;
                    max = tmp;
                    //pour qu'il prenne la première action qui le fait gagner pour gagner du temps
                    if (max == 1)
                    {
                        Console.WriteLine("IA should win");
                        return act;
                    }
                }
            }
            
            return act;
        }
     

        public float Max(BattleDecisionState s, int prof, int myId)
        {
            if (s.State.HasWinner() || prof == maxprof)
            {
                return eval(s.State, myId);
            }

            float max = -100;
            float tmp = -1 ;

            List<BattleDecisionState> children = s.GetChildren(myId);

            foreach (BattleDecisionState sc in children)
            {
                tmp = Min(sc, prof + 1, myId);
                if (tmp > max)
                {
                    max = tmp;
                }
            }
            return max;
        }


        public float Min(BattleDecisionState s, int prof, int myId)
        {
            int otherId = 1 - myId;

            if (s.State.HasWinner()|| prof==maxprof)
            {
                   return eval(s.State, myId); }

            float min = 100;
            float tmp = -1;

            List<BattleDecisionState> children = s.GetChildren(otherId);

            foreach (BattleDecisionState sc in children)
            {
                tmp = Max(sc,prof+1, myId);
                if (tmp < min)
                {
                    min = tmp;
                }
            }
            return min;
        }
    
        public float eval(BattleState s, int myID)
        {
            Trainer other = s.Trainers[1 - myID];
            Trainer me = s.Trainers[myID];
            int o = getLifeTotal(other);
            int m = getLifeTotal(me);

            return (float)m / (o + m);
        }

        private int getLifeTotal(Trainer t)
        {
            int res = 0;
            foreach(Pokemon p in t.Team)
            {
                res += p.CurrHP;
            }
            return res;
        }

        public override Trainer MakeTeam(List<Pokemon> availablePokemon, bool allowDoubles = false, int nbPokemon = 3)
        {
            //pour l'instant j'ai copié/collé le code de dumbAI, je verrais après pour la construction de team
            if (availablePokemon.Count >= nbPokemon)
            {
                trainer.Team.Clear();
                for (int i = 0; i < nbPokemon; ++i)
                    trainer.Team.Add(availablePokemon[i]);

                trainer.ActivePokemon = trainer.Team[0];
            }
            return Trainer;
        }
    }
}
