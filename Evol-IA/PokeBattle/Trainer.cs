using PokeMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattle
{
    public abstract class Trainer
    {
        public string Name { get; set; }

        public List<Pokemon> Team { get; set; }

        public Pokemon ActivePokemon { get; set; }

        public Trainer(string name, List<Pokemon> team)
        {
            Name = name;
            Team = team;
            if(Team.Count > 0)
                ActivePokemon = Team[0];
        }

        public abstract BattleAction ChooseAction();

        public abstract FightAction ChooseMove();

        public abstract PokemonAction ChoosePokemon();

        public bool IsOutOfPokemon()
        {
            foreach(Pokemon p in Team)
            {
                if (!p.Ko())
                    return false;
            }
            return true;
        }
    }
}
