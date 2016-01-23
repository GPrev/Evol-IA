using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeRules;

namespace Evol_IA
{
    public class MctsAI : BattleAI
    {
        const double absoluteWin = 1;
        const double absoluteLoss = 0;
        const int maxVisits = int.MaxValue/2;

        public Trainer trainer;
        int maxiter;
        int nbSimuPerIter;

        int totalVisits;

        Random r = new Random();

        public override Trainer Trainer
        {
            get { return trainer; }
        }

        public MctsAI(Trainer t = null)
        {
            if (t == null)
                trainer = new Trainer("MCTS AI", new List<Pokemon>());
            else
                trainer = t;

            maxiter = 40;
            nbSimuPerIter = 10;
        }

        public override Trainer MakeTeam(List<Pokemon> availablePokemon, bool allowDoubles = false, int nbPokemon = 3)
        {
            throw new NotImplementedException();
        }

        public override BattleAction ChooseAction(BattleDecisionState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            MctsNode root = new MctsNode(s);

            // 4 steps : selection, rollout and update
            for (totalVisits = 1; totalVisits <= maxiter; totalVisits++)
            {
                MctsNode current = Selection(root, myId);
                double Value = Rollout(current, myId);
                Update(current, Value, myId);
            }

            //TEST
            Console.WriteLine("=================");
            Console.WriteLine(root.ToGrapVizString());
            Console.WriteLine("=================");
            //TEST

            // Chooses best node
            return BestChildUCB(root, 0).Action;
        }

        // Selects the best child (by UCB) and continues until not all children exist or there is a winner
        private MctsNode Selection(MctsNode current, int myId)
        {
            BattleDecisionState state = current.State;
            int id = myId;
            while (!state.State.HasWinner())
            {
                List<BattleAction> actions = state.State.GetNextActions(id, true);

                if (actions.Count > current.Children.Count)
                    return Expand(current, myId);
                //else
                current = BestChildUCB(current);
                state = current.State;
                id = Opponent(id);
            }

            return current;
        }

        // Chooses the best child by UCB
        private MctsNode BestChildUCB(MctsNode current, double C = 1.44)
        {
            MctsNode bestChild = null;
            double bestUCB = double.NegativeInfinity;

            foreach (MctsNode child in current.Children)
            {
                double myUCB = UCB(child, C);

                if (myUCB > bestUCB)
                {
                    bestChild = child;
                    bestUCB = myUCB;
                }
            }

            return bestChild;
        }

        // Evaluates which child node should be expanded. With C=0, evaluates which node should be chosen
        private double UCB(MctsNode n, double C = 1.44)
        {
            return ((double)n.Value / (double)n.Visits) + C * Math.Sqrt((2.0 * Math.Log((double)totalVisits)) / (double)n.Visits);
        }

        // Makes a new child to the node and returns it
        private MctsNode Expand(MctsNode current, int myId)
        {
            BattleDecisionState state = current.State;

            int id;
            if (current.Action == null) // At root, start with first player
                id = myId;
            else
                id = Opponent(current.Action.GetActorId());

            List<BattleAction> actions = state.State.GetNextActions(id, true);

            // Remove already evaluated moves
            for (int i = 0; i < actions.Count; i++)
            {
                if (current.Children.Exists(c => c.Action.Equals(actions[i])))
                {
                    actions.RemoveAt(i);
                    i--;
                }
            }
            //Choses one option at random (there should be at least 1 action left)
            int newActionId = r.Next() % actions.Count;

            MctsNode node = current.MakeChild(actions[newActionId], id);
            return node;
        }

        // Makes random moves from the current node and returns the score
        private double Rollout(MctsNode current, int myId)
        {
            BattleDecisionState state = current.State;
            // If this state is terminal, we don't need to explore it more than once
            if (state.State.HasWinner())
            {
                current.Visits = maxVisits;

                int score1simu;
                if (state.State.Winner() == myId)
                    score1simu = 1;
                else
                    score1simu = 0;

                current.Value = maxVisits * score1simu;
                
                // If the winning player made the last decision, it will take it no matter what
                if(current.Action.GetActorId() == state.State.Winner())
                {
                    current.Parent.Visits = maxVisits;
                    current.Parent.Value = maxVisits * score1simu;
                }

                return score1simu;
            }

            //else
            double res = 0;

            for (int i = 0; i < nbSimuPerIter; ++i)
            {
                state = current.State;
                int id = Opponent(current.Action.GetActorId());

                // Loop until the match is over
                while (!state.State.HasWinner())
                {
                    //Random
                    List<BattleAction> actions = state.State.GetNextActions(id, true);
                    BattleAction a = actions[r.Next(0, actions.Count)];
                    state = state.GetChild(a, id);
                    id = Opponent(id);
                }

                if (state.State.Winner() == myId)
                    res += 1;
                //else 0
            }
            return res / nbSimuPerIter;
        }

        // Updates the current node and its parents with the result of the rollout (visits + 1, value + v)
        private void Update(MctsNode current, double value, int myId)
        {
            double reversed = ReverseScore(value);

            while (current != null)
            {
                current.Visits++;
                if(current.Action == null || current.Action.GetActorId() == myId)
                    current.Value += value;
                else // The enemy moves have a reversed score
                    current.Value += reversed;
                current = current.Parent;
            }
        }

        private int Opponent(int id)
        {
            return 1 - id;
        }

        // Converts the score of one player into the score of the other
        private double ReverseScore(double score)
        {
            return 1.0 - score;
        }
    }
}
