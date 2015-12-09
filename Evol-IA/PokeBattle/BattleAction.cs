using PokeMath;
using System;

namespace PokeBattle
{
    public enum ActionType { FIGHT, POKEMON, BAG, RUN }

    public abstract class BattleAction
    {
        public ActionType ActionType { get; protected set; }

        /// <summary>
        /// Returns the chosen move or pokemon, the other being null.
        /// </summary>
        public abstract Tuple<Move, Pokemon> GetMoveOrPokemon();
    }

    public class FightAction : BattleAction
    {
        public Move Move { get; set; }

        public FightAction()
        {
            ActionType = ActionType.FIGHT;
        }

        public override Tuple<Move, Pokemon> GetMoveOrPokemon()
        {
            return new Tuple<Move, Pokemon>(Move, null);
        }
    }

    public class PokemonAction : BattleAction
    {
        public Pokemon Pokemon { get; set; }

        public PokemonAction()
        {
            ActionType = ActionType.POKEMON;
        }

        public override Tuple<Move, Pokemon> GetMoveOrPokemon()
        {
            return new Tuple<Move, Pokemon>(null, Pokemon);
        }
    }
}