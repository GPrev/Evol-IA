using PokeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public class Trainer : ICloneable
    {
        public string Name { get; set; }

        public List<Pokemon> Team { get; set; }

        int activeID = 0;

        public virtual Pokemon ActivePokemon
        {
            get
            {
                if (Team.Count > activeID)
                    return Team[activeID];
                else
                    return null;
            }
            set
            {
                if(Team.Contains(value))
                    activeID = Team.IndexOf(value);
            }
        }

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

        public int getNumberOfPokemon()
        {
            int c=0;
            foreach (Pokemon p in Team)
            {
                if (!p.Ko())
                    c = c + 1;    
            }
            return c; 
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

            Trainer res =  new Trainer(Name, newTeam);
            res.activeID = this.activeID;
            return res;
        }
    }
}
