using PokeMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattle
{
    public class Trainer : ICloneable
    {
        public string Name { get; set; }

        public List<Pokemon> Team { get; set; }

        public virtual Pokemon ActivePokemon { get; set; }

        public Trainer(string name, List<Pokemon> team)
        {
            Name = name;
            Team = team;
            if(Team.Count > 0)
                ActivePokemon = Team[0];
        }

        public bool IsOutOfPokemon()
        {
            foreach(Pokemon p in Team)
            {
                if (!p.Ko())
                    return false;
            }
            return true;
        }

        public object Clone()
        {
            List<Pokemon> newTeam = new List<Pokemon>();
            Pokemon newActive = null;
            foreach (Pokemon p in Team)
            {
                Pokemon newP = p.Clone() as Pokemon;
                newTeam.Add(newP);
                if (p == ActivePokemon)
                    newActive = newP;

            }

            return new Trainer(Name, newTeam) { ActivePokemon = newActive };
        }
    }
}
