﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokeRules;

using Type = PokeRules.Type;
using System.Collections.Generic;

namespace PokeRulesTests
{
    [TestClass]
    public class G4RulesTests
    {
        private G4Rules rules;

        [TestInitialize]
        public void Initialize()
        {
            rules = new G4Rules();
        }

        [TestMethod]
        public void TypeMultiplierTest()
        {
            Assert.AreEqual(2, rules.GetTypeModifier(Type.ELECTRIC, Type.WATER));

            Assert.AreEqual(.5, rules.GetTypeModifier(Type.FIRE, Type.WATER));
        }

        [TestMethod]
        public void DamageFormulaTest()
        {
            Pokemon p1 = new Pokemon("a", 50, Type.ELECTRIC, Type.NONE, 100, 50, 50, 50, 50, 50, new List<Move>());
            Pokemon p2 = new Pokemon("b", 50, Type.WATER, Type.NONE, 100, 50, 50, 50, 50, 50, new List<Move>());
            Move m = new Move("m", Type.ELECTRIC, 50, 100, true);
            Move m2 = new Move("m2", Type.WATER, 50, 100, true);
            //http://nuggetbridge.com/damagecalc/
            Assert.IsTrue(60 <= rules.DamageFormula(p1, p2, m));
            Assert.IsTrue(72 >= rules.DamageFormula(p1, p2, m));

            Assert.IsTrue(30 <= rules.DamageFormula(p2, p1, m2));
            Assert.IsTrue(36 >= rules.DamageFormula(p2, p1, m2));
        }

        [TestMethod]
        public void MinDamageTest()
        {
            Pokemon attP = new Pokemon("b", 1, Type.WATER, Type.NONE, 20, 10, 10, 10, 10, 10, new List<Move>());
            Pokemon defP = new Pokemon("a", 100, Type.ELECTRIC, Type.NONE, 200, 100, 100, 100, 100, 100, new List<Move>());
            Move m = new Move("m", Type.NORMAL, 20, 100, true);
            Assert.AreEqual(1, rules.DamageFormula(attP, defP, m));
        }
    }
}
