﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    public abstract class Rules
    {
        public abstract int DamageFormula(Pokemon attP, Pokemon defP, Move m);
    }
}
