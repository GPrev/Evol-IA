﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    public abstract class Rules
    {
        public abstract void resolveTurn(Pokemon p1, Pokemon p2, Move a1, Move a2);
    }
}
