using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    public class G4Rules : Rules
    {
        // Multiplied by  4 (so 1 is 1/ 4). Do not call directly, use getTypeModifier(...) instead
        //                                      No  Fi  Fl  Po  Gr  Ro  Bu  Gh  St  Fi  Wa  Gr  El  Ps  Ic  Dr  Da  Fa
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

        public float getTypeModifier(Type attackType, Type defType1, Type defType2 = Type.NONE)
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
        public int damageFormula(Pokemon attP, Pokemon defP, Move m)
        {
            int att = attP.Attack;
            if (m.Special)
                att = attP.SpAttack;

            int def = attP.Defense;
            if (m.Special)
                def = attP.SpDefense;

            int mod1 = 1;
            int mod2 = 1;
            int mod3 = 1;

            int cc = 1;
            int r = 1;
            float stab = 1;
            if(attP.Type == m.Type || attP.Type2 == m.Type)
                stab = 1.5f;

            float typeMod = getTypeModifier(m.Type, defP.Type, defP.Type2);

            return (int)((((((((attP.Level * 2 / 5) + 2) * m.Power * att / 50) / def) * mod1) + 2) * cc * mod2 * r / 100) *stab * typeMod * mod3);
        }

        public override void resolveTurn(Pokemon p1, Pokemon p2, Move a1, Move a2)
        {
            throw new NotImplementedException();
        }
    }

}
