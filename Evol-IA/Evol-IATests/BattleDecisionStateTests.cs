using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Evol_IA;
using PokeRules;
using System.Collections.Generic;

namespace Evol_IATests
{
    [TestClass]
    public class BattleDecisionStateTests
    {
        BattleDecisionState s;

        [TestInitialize]
        public void Initialize()
        {
            Pokemon p1 = new Pokemon("a", 50, PokeRules.Type.ELECTRIC, PokeRules.Type.NONE, 100, 50, 50, 50, 50, 50, new List<Move>());
            Pokemon p2 = new Pokemon("b", 50, PokeRules.Type.WATER, PokeRules.Type.NONE, 100, 50, 50, 50, 50, 50, new List<Move>());
            p1.Moves.Add(new Move("m1", PokeRules.Type.ELECTRIC, 10, 100, true));
            p2.Moves.Add(new Move("m2", PokeRules.Type.WATER, 10, 100, true));

            List<Pokemon> TeamA = new List<Pokemon>();
            List<Pokemon> TeamB = new List<Pokemon>();
            TeamA.Add(p1);
            TeamB.Add(p2);

            List<Trainer> trainers = new List<Trainer>();
            trainers.Add(new Trainer("Red", TeamA));
            trainers.Add(new Trainer("Blue", TeamB));

            s = new BattleDecisionState(new BattleState(trainers));
        }

        [TestMethod]
        public void GetChildTest()
        {
            // First action
            BattleAction a1 = s.State.GetNextActions(0)[0];
            BattleDecisionState newS = s.GetChild(a1, 0);
            Assert.AreEqual(s.State, newS.State);

            // Second action
            BattleAction a2 = s.State.GetNextActions(1)[0];
            BattleDecisionState newNewS = newS.GetChild(a2, 1);

            List<BattleAction> actions = new List<BattleAction>();
            actions.Add(a1);
            actions.Add(a2);
            BattleState expected = s.State.Clone() as BattleState;
            expected.MakeActions(actions);

            Assert.AreNotEqual(s.State, newNewS.State);
            Assert.AreEqual(expected.Trainers[0].ActivePokemon.CurrHP, newNewS.State.Trainers[0].ActivePokemon.CurrHP);
            Assert.AreEqual(expected.Trainers[1].ActivePokemon.CurrHP, newNewS.State.Trainers[1].ActivePokemon.CurrHP);

            // Third action
            BattleAction a3 = s.State.GetNextActions(0)[0];
            BattleDecisionState newNewNewS = newNewS.GetChild(a3, 0);
            Assert.AreEqual(newNewS.State, newNewNewS.State);
        }
    }
}
