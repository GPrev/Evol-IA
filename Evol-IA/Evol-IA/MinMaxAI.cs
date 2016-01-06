using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeRules;
using PokeRules;

namespace Evol_IA
{
    public class MinMaxAI : BattleAI
    {
        public Trainer trainer;

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
        }

        public override BattleAction ChooseAction(BattleDecisionState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            int max = -10000;
            int tmp;
            BattleAction act=null;

            List<BattleAction> actions = s.State.GetNextActions(myId);
            BattleState sclone = (BattleState) s.State.Clone();

            foreach (BattleAction a in actions)
            {
                //  List<BattleAction> l = new List<BattleAction>();
                //    l.Add(a);

                //      sclone.MakeActions(l);
                Console.WriteLine("foreach");
                tmp = Min(sclone,a);
                if (tmp> max)
                {
                    act = a;
                }
            }

            return act;
        }
     

        public int Max(BattleState s, int myId = 1)
        {
            if (s.HasWinner())
            { if (trainer.IsOutOfPokemon())   return 0;
                return 1;}

            int max = -100;
            int tmp = -1 ;
           
            List<BattleAction> actions = s.GetNextActions(myId);
            BattleState sclone = (BattleState) s.Clone();
            foreach (BattleAction a in actions)
                {
                    tmp = Min(sclone,a);
                    if (tmp > max)
                    {
                    max = tmp;
                    }
                }
            return max;
        }


        public int Min(BattleState s, BattleAction amax, int myId = 0)
        {

            if (s.HasWinner())
            {  if (trainer.IsOutOfPokemon()) return 1;
                return 0;  }
            int min = 100;
            int tmp = -1;

            List<BattleAction> actions = s.GetNextActions(myId);
            BattleState sclone = (BattleState) s.Clone();

            foreach (BattleAction a in actions)
            {
                Console.WriteLine("foreach min");
                List<BattleAction> l = new List<BattleAction>();
                l.Add(a);
                l.Add(amax);

                sclone.MakeActions(l);
                tmp =Max(sclone);
                if (tmp < min)
                {
                    min = tmp;
                }
            }
            return min;
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
