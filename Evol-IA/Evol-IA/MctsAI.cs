﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeRules;

namespace Evol_IA
{
    class MctsAI : BattleAI
    {
        // Regular values are positive so these values are safe
        const double absoluteWin = -1;
        const double absoluteLoss = -2;

        public Trainer trainer;
        int maxiter;

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

            maxiter = 100;
        }

        public override Trainer MakeTeam(List<Pokemon> availablePokemon, bool allowDoubles = false, int nbPokemon = 3)
        {
            throw new NotImplementedException();
        }

        public override BattleAction ChooseAction(BattleDecisionState s, int myId = 1, ActionType type = ActionType.ANY)
        {
            MctsNode root = new MctsNode(s);

            // 4 steps : selection, rollout and update
            for (int iteration = 0; iteration < 1000; iteration++)
            {
                MctsNode current = Selection(root, myId);
                double Value = Rollout(current, myId);
                Update(current, Value, myId);
            }
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
                List<BattleAction> actions = state.State.GetNextActions(id);

                if (actions.Count > current.Children.Count)
                    return Expand(current, myId);
                //else
                current = BestChildUCB(current);
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
            return ((double)n.Value / (double)n.Visits) + C * Math.Sqrt((2.0 * Math.Log((double)n.Visits)) / (double)n.Visits);
        }

        // Makes a new child to the node and returns it
        private MctsNode Expand(MctsNode current, int myId)
        {
            BattleDecisionState state = current.State;
            int id = current.Action.GetActorId();

            List<BattleAction> actions = state.State.GetNextActions(id);

            for (int i = 0; i < actions.Count; i++)
            {
                //We already have evaluated this move
                if (current.Children.Exists(c => c.Action == actions[i]))
                    continue;

                id = Opponent(id);

                MctsNode node = current.MakeChild(actions[i], id);

                return node;
            }
            // This shouldn't be reached
            return null;
        }

        // Makes random moves from the current node and returns the score
        private double Rollout(MctsNode current, int myId)
        {
            BattleDecisionState state = current.State;
            //If this state is a loss, we shouldn't reach the parent state
            if (state.State.Winner() == Opponent(myId))
            {
                current.Parent.Value = int.MinValue;
                return 0;
            }

            Random r = new Random();
            int id = Opponent(current.Action.GetActorId());

            // Loop until the match is over
            while (state.State.Winner() == 0)
            {
                //Random
                List<BattleAction> actions = state.State.GetNextActions(id);
                BattleAction a = actions[r.Next(0, actions.Count)];
                state = state.GetChild(a, id);
                id = Opponent(id);
            }

            if (state.State.Winner() == myId)
                return 1;

            return 0;
        }

        // Updates the current node and its parents with the result of the rollout (visits + 1, value + v)
        private void Update(MctsNode current, double value, int myId)
        {
            double reversed = ReverseScore(value);

            while (current != null)
            {
                current.Visits++;
                if(current.Action.GetActorId() == myId)
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
            return 1 - score;
        }
    }
}
