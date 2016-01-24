using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public class G4Rules : Rules
    {
        // Multiplied by  4 (so 1 is 1/ 4). Do not call directly, use getTypeModifier(...) instead
        //                                                  No  Fi  Fl  Po  Gr  Ro  Bu  Gh  St  Fi  Wa  Gr  El  Ps  Ic  Dr  Da  Fa
        static readonly int[,] typeChart = new int[,]   { {  4,  4,  4,  4,  4,  2,  4,  0,  2,  4,  4,  4,  4,  4,  4,  4,  4,  4 },   // No
                                                          {  8,  4,  2,  2,  4,  8,  2,  0,  8,  4,  4,  4,  4,  2,  8,  4,  8,  2 },   // Fi
                                                          {  4,  8,  4,  4,  4,  2,  8,  4,  2,  4,  4,  8,  2,  4,  4,  4,  4,  4 },   // Fl
                                                          {  4,  4,  4,  2,  2,  2,  4,  2,  0,  4,  4,  8,  4,  4,  4,  4,  4,  8 },   // Po
                                                          {  4,  4,  0,  8,  4,  8,  2,  4,  8,  8,  4,  2,  8,  4,  4,  4,  4,  4 },   // Gr
                                                          {  4,  2,  8,  4,  2,  4,  8,  4,  2,  8,  4,  4,  4,  4,  8,  4,  4,  4 },   // Ro
                                                          {  4,  2,  2,  2,  4,  4,  4,  2,  2,  2,  4,  8,  4,  8,  4,  4,  8,  2 },   // Bu
                                                          {  0,  4,  4,  4,  4,  4,  4,  8,  4,  4,  4,  4,  4,  8,  4,  4,  2,  4 },   // Gh
                                                          {  4,  4,  4,  4,  4,  8,  4,  4,  2,  2,  2,  4,  2,  4,  8,  4,  4,  8 },   // St
                                                          {  4,  4,  4,  4,  4,  2,  8,  4,  8,  2,  2,  8,  4,  4,  8,  2,  4,  4 },   // Fi
                                                          {  4,  4,  2,  2,  8,  8,  4,  4,  4,  8,  2,  2,  4,  4,  4,  2,  4,  4 },   // Wa
                                                          {  4,  4,  4,  0,  8,  8,  2,  4,  2,  2,  8,  2,  4,  4,  4,  2,  4,  4 },   // Gr
                                                          {  4,  4,  8,  4,  0,  4,  4,  4,  4,  4,  8,  2,  2,  4,  4,  2,  4,  4 },   // El
                                                          {  4,  8,  4,  8,  4,  4,  4,  4,  2,  4,  4,  4,  4,  2,  4,  4,  0,  4 },   // Ps
                                                          {  4,  4,  8,  4,  8,  4,  4,  4,  2,  2,  2,  8,  4,  4,  2,  8,  4,  4 },   // Ic
                                                          {  4,  4,  4,  4,  4,  4,  4,  4,  2,  4,  4,  4,  4,  4,  4,  8,  4,  0 },   // Dr
                                                          {  4,  2,  4,  4,  4,  4,  4,  8,  4,  4,  4,  4,  4,  8,  4,  4,  2,  2 },   // Da
                                                          {  4,  8,  4,  2,  4,  4,  4,  4,  2,  2,  4,  4,  4,  4,  4,  8,  8,  4 } }; // Fa

        public override float GetTypeModifier(Type attackType, Type defType1, Type defType2 = Type.NONE)
        {
            if (defType2 == Type.NONE)
                return (float)typeChart[(int)attackType,(int)defType1] / 4;
            else
            {
                int m1 = typeChart[(int)attackType, (int)defType1];
                int m2 = typeChart[(int)attackType, (int)defType2];
                return (float)(m1 * m2) / 16;
            }
        }

        //http://www.pokebip.com/pokemon/page__jeuxvideo__guide_tactique_strategie_pokemon__formules_mathematiques.html
        public override int DamageFormula(Pokemon attP, Pokemon defP, Move m)
        {
            if (m.Power == 0)
                return 0;

            int att;
            if (m.Special)
                att = attP.SpAttack;
            else
                att = attP.Attack;

            int def;
            if (m.Special)
                def = defP.SpDefense;
            else
                def = defP.Defense;

            int mod1 = 1;
            int mod2 = 1;
            int mod3 = 1;

            int cc = 1; //no criticals
            int r = 92; //no random damage reductions (85 < r < 100)
            float stab = 1;
            if(attP.Type == m.Type || attP.Type2 == m.Type)
                stab = 1.5f;

            float typeMod = GetTypeModifier(m.Type, defP.Type, defP.Type2);

            int res = (int)((float)attP.Level * 2 / 5) + 2;
            res = (int)((float)res * m.Power * att / 50);
            res /= def;
            res *= mod1;
            res += 2;
            res = (int)((float)res * cc * mod2 * r / 100);
            res *= (int)(stab * typeMod * mod3);

            if (res < 1)
                res = 1;

            return res;
        }

        public override int FasterThan(ActionType a1, ActionType a2)
        {
            // Fight goes after the rest, else we don't care
            if (a1 == a2)
                return 0;
            //else
            if (a1 == ActionType.FIGHT)
                return 1;
            //else
            if (a2 == ActionType.FIGHT)
                return -1;
            //else
            return 0;
        }

        public override int FasterThan(BattleState s, FightAction a1, FightAction a2)
        {
            // Compare speeds
            return a2.getAttacker(s).Speed - a1.getAttacker(s).Speed;
        }

        public override bool CanApplyCondition(Condition condition, Pokemon pokemon)
        {
            if (pokemon.Condition != Condition.OK)
                return false;
            //else
            switch(condition)
            {
                case Condition.POISONED: // Poison and Steel pokemon can't be poisoned
                    return !((pokemon.HasType(Type.STEEL) || pokemon.HasType(Type.POISON)));

                default:
                    return true;
            }
        }
    }

}
