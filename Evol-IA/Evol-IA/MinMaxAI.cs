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
        int myId;

        public override Trainer Trainer
        {
            get { return trainer; }
        }

        /*modif de MinMax AI
        On peut choisir le nombre d'itérations dans le constructreur
        Le myId n'est plus en paramètres dans chaque fonction mais est un attribut de l'IA
        Il est ajouté à la création (1 par défaut)
        Pour ChooseAction, il a toujours en paramètres myID. Il sert à rien mais sinon il ralait niveau héritage. Et vu qu'on le mettait toujours à myId...
        J'ai testé, normalement ça marche aussi bien qu'avant
        J'ai juste laissé en commentaires les anciennes déclarations de fonctions au cas ou
        */
        public MinMaxAI(Trainer t = null, int m=5) //on peut choisir le nombre d'itérations dans le constructeur
        {
            if (t == null)
                trainer = new Trainer("MinMax AI", new List<Pokemon>());
            else
                trainer = t;
            maxprof = m;
        }

        //        public override BattleAction ChooseAction(BattleDecisionState s, int myId = 1, ActionType type = ActionType.ANY)
        public override BattleAction ChooseAction(BattleDecisionState s, int myID, ActionType type = ActionType.ANY)
        {
            //Console.WriteLine("In MinMaxIA");
            this.myId = myID;
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
                    //pour qu'il prenne la première action qui le fait gagner pour gagner du temps
                    if (max == 1)
                    {
                        return act;
                    }
                }
            }
            
            return act;
        }

        //        public float Max(BattleDecisionState s, int prof, int myId)

        public float Max(BattleDecisionState s, int prof)
        {
            if (s.State.HasWinner() || prof == maxprof)
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

        //        public float Min(BattleDecisionState s, int prof, int myId)

        public float Min(BattleDecisionState s, int prof)
        {
            int otherId = 1 - myId;

            if (s.State.HasWinner()|| prof==maxprof)
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
        //        public float eval(BattleState s, int myID)

        public float eval(BattleState s)
        {
            Trainer other = s.Trainers[1 - myId];
            Trainer me = s.Trainers[myId];
            int o = getLifeTotal(other);
            int m = getLifeTotal(me);

            float res= (float)m / (o + m);
            if (res == 0.0)
            {
                int om = getMaxTotalLife(other);
                return (float) -(o / om);
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
