using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokeRules;
using System.Collections.Generic;

namespace PokeMathTests
{
    [TestClass]
    public class TrainerTests
    {
        Trainer t;

        [TestInitialize]
        public void Initialize()
        {
            Pokemon p = new Pokemon("a", 50, PokeRules.Type.ELECTRIC, PokeRules.Type.NONE, 100, 50, 50, 50, 50, 50, new List<Move>());
            p.Moves.Add(new Move("m1", PokeRules.Type.ELECTRIC, 10, 100, true));

            List<Pokemon> l = new List<Pokemon>();
            l.Add(p);
            t = new Trainer("t", l);
        }

        [TestMethod]
        public void CloneTests()
        {
            Trainer t2 = t.Clone() as Trainer;
            Assert.IsNotNull(t2);

            t2.ActivePokemon.CurrHP -= 10;
            Assert.AreEqual(90, t2.ActivePokemon.CurrHP);
            // Checks that the original Pokemon is not affected
            Assert.AreEqual(100, t.ActivePokemon.CurrHP);
        }
    }
}
