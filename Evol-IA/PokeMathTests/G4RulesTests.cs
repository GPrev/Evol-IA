using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokeMath;

using Type = PokeMath.Type;

namespace PokeMathTests
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

            Assert.AreEqual(.5, rules.GetTypeModifier(Type.WATER, Type.ELECTRIC));
        }

        [TestMethod]
        public void DamageFormulaTest()
        {
            Pokemon attP = new Pokemon("a", 50, Type.ELECTRIC, Type.NONE, 100, 50, 50, 50, 50, 50);
            Pokemon defP = new Pokemon("b", 50, Type.WATER, Type.NONE, 100, 50, 50, 50, 50, 50);
            Move m = new Move("m", Type.ELECTRIC, 50, 100, true);
            //http://nuggetbridge.com/damagecalc/
            Assert.IsTrue(60 <= rules.DamageFormula(attP, defP, m));
            Assert.IsTrue(72 >= rules.DamageFormula(attP, defP, m));
        }

        [TestMethod]
        public void MinDamageTest()
        {
            Pokemon attP = new Pokemon("b", 1, Type.WATER, Type.NONE, 20, 10, 10, 10, 10, 10);
            Pokemon defP = new Pokemon("a", 100, Type.ELECTRIC, Type.NONE, 200, 100, 100, 100, 100, 100);
            Move m = new Move("m", Type.NORMAL, 20, 100, true);
            Assert.AreEqual(1, rules.DamageFormula(attP, defP, m));
        }
    }
}
