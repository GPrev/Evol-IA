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
        }

        public override BattleAction ChooseAction(BattleDecisionState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            maxprof = 10;
            int max = -10000;
            int tmp;
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
                       if (max == 5)
                    {
                        return act; 
                    }


                }
            }
            
            return act;
        }
     

        public int Max(BattleDecisionState s, int prof, int myId = 1)
        {
            if (s.State.HasWinner() || prof == maxprof)
            {
                return eval(s.State);
            }

            int max = -100;
            int tmp = -1 ;
           
            List<BattleAction> actions = s.State.GetNextActions(myId);

            foreach (BattleAction a in actions)
                {
                      BattleDecisionState sc = s.GetChild(a, myId);
                    tmp = Min(sc,prof+1);
                    if (tmp > max)
                    {
                    max = tmp;
                    }
                }
            return max;
        }


        public int Min(BattleDecisionState s, int prof, int myId = 0)
        {

            if (s.State.HasWinner()|| prof==maxprof)
            {
                   return eval(s.State); }

            int min = 100;
            int tmp = -1;

            List<BattleAction> actions = s.State.GetNextActions(myId);

            foreach (BattleAction a in actions)
            {
                BattleDecisionState sc = s.GetChild(a, myId);
                tmp = Max(sc,prof+1);
                if (tmp < min)
                {
                    min = tmp;
                }
            }
            return min;
        }
    
        public int eval(BattleState s)
        {
            if (trainer.IsOutOfPokemon())
            {
                Console.WriteLine("IA Loses");
                return 0; //0 si l'IA n'a plus de pokémons
            }
            else if (s.HasWinner())
            {
                Console.WriteLine("IA Wins");
                return 5; //5 si il y a un vainqueur et que l'IA a encore des pokémons
            }
            else {                
                        return 1;
            } //1 autrement (l'arbre n'a pas été parcouru en entier)
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
