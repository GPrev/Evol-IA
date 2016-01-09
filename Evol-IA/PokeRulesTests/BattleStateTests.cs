using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokeRules;
using System.Collections.Generic;

namespace PokeMathTests
{
    [TestClass]
    public class BattleStateTests
    {
        BattleState s;
        BattleAction a1;
        BattleAction a2;

        [TestInitialize]
        public void Initialize()
        {
            Pokemon p1 = new Pokemon("a", 50, PokeRules.Type.ELECTRIC, PokeRules.Type.NONE, 100, 50, 50, 50, 50, 50, new List<Move>());
            Pokemon p2 = new Pokemon("b", 50, PokeRules.Type.WATER, PokeRules.Type.NONE, 100, 50, 50, 50, 50, 50, new List<Move>());
            p1.Moves.Add(new Move("m1", PokeRules.Type.ELECTRIC, 10, 100, true));
            p2.Moves.Add(new Move("m2", PokeRules.Type.WATER, 10, 100, true));
            p2.Moves.Add(new Move("m3", PokeRules.Type.NORMAL, 10, 100, true));

            List<Pokemon> TeamA = new List<Pokemon>();
            List<Pokemon> TeamB = new List<Pokemon>();
            TeamA.Add(p1);
            TeamB.Add(p2);

            List<Trainer> trainers = new List<Trainer>();
            trainers.Add(new Trainer("Red", TeamA));
            trainers.Add(new Trainer("Blue", TeamB));

            s = new BattleState(trainers);

            a1 = s.GetNextActions(0)[0];
            a2 = s.GetNextActions(1)[0];
        }

        [TestMethod]
        public void CanMakeActionsTests()
        {
            List<BattleAction> goodList = new List<BattleAction>();
            List<BattleAction> badList = new List<BattleAction>();

            goodList.Add(a1);
            goodList.Add(a2);

            badList.Add(a2);
            badList.Add(a1);

            Assert.IsFalse(s.CanMakeActions(badList));
            Assert.IsTrue(s.CanMakeActions(goodList));
        }

        [TestMethod]
        public void MakeActionsTests()
        {
            List<BattleAction> list = new List<BattleAction>();

            list.Add(a1);
            list.Add(a2);

            s.MakeActions(list);

            Assert.AreNotEqual(100, s.Trainers[0].ActivePokemon.CurrHP);
            Assert.AreNotEqual(100, s.Trainers[1].ActivePokemon.CurrHP);
        }

        [TestMethod]
        public void PossibleActionsTests()
        {
            List<BattleAction> list1 = s.GetNextActions(0);
            List<BattleAction> list2 = s.GetNextActions(1);

            Assert.AreEqual(1, list1.Count);
            Assert.AreEqual(2, list2.Count);
        }

        [TestMethod]
        public void CloneTests()
        {
            BattleState s2 = s.Clone() as BattleState;
            Assert.IsNotNull(s2);

            List<BattleAction> list = new List<BattleAction>();

            list.Add(a1);
            list.Add(a2);

            s2.MakeActions(list);

            // Checks that the original state is not affected
            Assert.AreEqual(100, s.Trainers[0].ActivePokemon.CurrHP);
            Assert.AreEqual(100, s.Trainers[1].ActivePokemon.CurrHP);
        }
    }
}