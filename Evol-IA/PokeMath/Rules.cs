using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    public abstract class Rules
    {
        public abstract int DamageFormula(Pokemon attP, Pokemon defP, Move m);

        public abstract float GetTypeModifier(Type attackType, Type defType1, Type defType2 = Type.NONE);

        public float GetTypeModifier(Move m, Pokemon p)
        {
            return GetTypeModifier(m.Type, p.Type, p.Type2);
        }

        public abstract bool FasterThan(Pokemon p1, Pokemon p2, Move m1, Move m2);

        public List<int> OrderMoves(List<Pokemon> pokemon, List<Move> moves)
        {
            // spots[i] = n means pokemon i will play after n others
            List<int> spots = new List<int>();

            // Makes spots the right size
            for (int i = 0; i < pokemon.Count; ++i)
                spots.Add(0);

            for (int i = 0; i < pokemon.Count; ++i)
            {
                for (int j = i + 1; j < pokemon.Count; ++j)
                {
                    if (FasterThan(pokemon[i], pokemon[j], moves[i], moves[j]))
                        spots[j]++;
                    else
                        spots[i]++;
                }
            }
            // This will contain the pokemon ids ordered by speed
            List<int> result = new List<int>();

            // Makes result the right size
            for (int i = 0; i < pokemon.Count; ++i)
                result.Add(0);

            for (int i = 0; i < pokemon.Count; ++i)
            {
                result[spots[i]] = i;
            }
            return result;
        }
    }
}
