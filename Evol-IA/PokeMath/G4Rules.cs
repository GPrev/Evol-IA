﻿using System;
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
            int att = attP.Attack;
            if (m.Special)
                att = attP.SpAttack;

            int def = attP.Defense;
            if (m.Special)
                def = attP.SpDefense;

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
            Console.WriteLine("Res : " + res);
            res = (int)((float)res * m.Power * att / 50);
            Console.WriteLine("Res : " + res);
            res /= def;
            Console.WriteLine("Res : " + res);
            res *= mod1;
            Console.WriteLine("Res : " + res);
            res += 2;
            Console.WriteLine("Res : " + res);
            res = (int)((float)res * cc * mod2 * r / 100);
            Console.WriteLine("Res : " + res);
            res *= (int)(stab * typeMod * mod3);
            Console.WriteLine("Res : " + res);

            if (res < 1)
                res = 1;

            return res;
        }

        public override bool FasterThan(Pokemon p1, Pokemon p2, Move m1, Move m2)
        {
            // If same speed, chooses at random
            if (p1.Speed == p2.Speed)
                return (new Random().Next()) % 2 == 0;
            //else
            return p1.Speed > p2.Speed;
        }
    }

}
