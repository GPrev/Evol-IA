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
        int maxdepth;
        int myId;

        public override Trainer Trainer
        {
            get { return trainer; }
        }

        //m is the maximal depth of the min max tree, and id is the id of the IA "player"
        public MinMaxAI(Trainer t = null, int m=5, int id=1)
        {
            if (t == null)
                trainer = new Trainer("MinMax AI", new List<Pokemon>());
            else
                trainer = t;
            myId=id;
            maxdepth = m;
        }

        public override BattleAction ChooseAction(BattleDecisionState s, int myID, ActionType type = ActionType.ANY)
        {
            float max = -10000;
            float tmp;
            BattleAction act=null;

            List<BattleAction> actions = s.State.GetNextActions(myId);

            foreach (BattleAction a in actions)
            {

                BattleDecisionState sc = s.GetChild(a, myId);
                tmp = Min(sc, 0);

                if (tmp> max)
                {
                    act = a;
                    max = tmp;
                }
            }
            
            return act;
        }


        public float Max(BattleDecisionState s, int prof)
        {
            if (s.State.HasWinner() || prof == maxdepth)
            {
                return eval(s.State);
            }

            float max = -100;
            float tmp = -1 ;

            List<BattleDecisionState> children = s.GetChildren(myId);

            foreach (BattleDecisionState sc in children)
            {
                tmp = Min(sc, prof + 1);
                if (tmp > max)
                {
                    max = tmp;
                }
            }
            return max;
        }


        public float Min(BattleDecisionState s, int prof)
        {
            int otherId = 1 - myId;

            if (s.State.HasWinner()|| prof==maxdepth)
            {
                   return eval(s.State); }

            float min = 100;
            float tmp = -1;

            List<BattleDecisionState> children = s.GetChildren(otherId);

            foreach (BattleDecisionState sc in children)
            {
                tmp = Max(sc,prof+1);
                if (tmp < min)
                {
                    min = tmp;
                }
            }
            return min;
        }


        public float eval(BattleState s)
        {
            Trainer other = s.Trainers[1 - myId];
            Trainer me = s.Trainers[myId];
            int o = getLifeTotal(other);
            int m = getLifeTotal(me);

            //IA tries to keep the biggest amount of life possible while trying to do the biggest damages possible
            float res= (float)m / (o + m);

            if (res == 0.0) //if the IA thinks it will lose
            {
                int om = getMaxTotalLife(other);

                return (float) (-1)*(o / om); //it tries to do as much damages as it can, so it may still win if the ennemy makes a mistake
            }

            return res;
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

        private int getMaxTotalLife(Trainer t)
        {
            int res = 0;
            foreach(Pokemon p in t.Team)
            {
                res += p.HP;
            }
            return res;
        }


        //currently not used
        public override Trainer MakeTeam(List<Pokemon> availablePokemon, bool allowDoubles = false, int nbPokemon = 3)
        {
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
