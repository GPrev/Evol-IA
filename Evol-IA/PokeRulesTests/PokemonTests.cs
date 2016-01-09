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
    }
}
