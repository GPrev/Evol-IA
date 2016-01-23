using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokeRules;
using System.Collections.Generic;

namespace PokeMathTests
{
    [TestClass]
    public class PokemonTests
    {
        Pokemon p;

        [TestInitialize]
        public void Initialize()
        {
            p = new Pokemon("a", 50, PokeRules.Type.ELECTRIC, PokeRules.Type.NONE, 100, 50, 50, 50, 50, 50, new List<Move>());
            p.Moves.Add(new Move("m1", PokeRules.Type.ELECTRIC, 10, 100, true));
        }

        [TestMethod]
        public void CloneTests()
        {
            Pokemon p2 = p.Clone() as Pokemon;
            Assert.IsNotNull(p2);

            p2.CurrHP -= 10;
            Assert.AreEqual(90, p2.CurrHP);
            // Checks that the original Pokemon is not affected
            Assert.AreEqual(100, p.CurrHP);
        }

        [TestMethod]
        public void StatModifiersInitTests()
        {
            Assert.AreEqual(0, p.GetStatModifier(Statistic.ATTACK));
            Assert.AreEqual(0, p.GetStatModifier(Statistic.DEFENSE));
            Assert.AreEqual(0, p.GetStatModifier(Statistic.SPATTACK));
            Assert.AreEqual(0, p.GetStatModifier(Statistic.SPDEFENSE));
            Assert.AreEqual(0, p.GetStatModifier(Statistic.SPEED));
        }

        [TestMethod]
        public void StatModifiersAddTests()
        {
            p.AddStatModifier(Statistic.SPATTACK, 3);
            Assert.AreEqual(3, p.GetStatModifier(Statistic.SPATTACK));

            p.AddStatModifier(Statistic.SPATTACK, -2);
            Assert.AreEqual(1, p.GetStatModifier(Statistic.SPATTACK));
        }

        [TestMethod]
        public void StatModifiersSetTests()
        {
            p.SetStatModifier(Statistic.SPATTACK, 3);
            Assert.AreEqual(3, p.GetStatModifier(Statistic.SPATTACK));

            p.SetStatModifier(Statistic.SPDEFENSE, -6);
            Assert.AreEqual(-6, p.GetStatModifier(Statistic.SPDEFENSE));
            Assert.AreEqual(3, p.GetStatModifier(Statistic.SPATTACK));

            p.SetStatModifier(Statistic.SPEED, 6);
            Assert.AreEqual(-6, p.GetStatModifier(Statistic.SPDEFENSE));
            Assert.AreEqual(3, p.GetStatModifier(Statistic.SPATTACK));
            Assert.AreEqual(6, p.GetStatModifier(Statistic.SPEED));
        }
    }
}
