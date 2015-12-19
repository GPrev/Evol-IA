using PokeMath;
using System;

namespace PokeBattle
{
    public enum ActionType { FIGHT, POKEMON, BAG, RUN, ANY, NONE }

    public abstract class BattleAction
    {
        public ActionType ActionType { get; protected set; }

        /// <summary>
        /// Returns the chosen move or pokemon, the other being null.
        /// </summary>
        public abstract Tuple<Move, Pokemon> GetMoveOrPokemon();

        public Move GetMove() { return GetMoveOrPokemon().Item1; }
        public Pokemon GetPokemon() { return GetMoveOrPokemon().Item2; }

        public abstract String GetMessage();
    }

    public class FightAction : BattleAction
    {
        public Pokemon Attacker { get; set; }
        public Pokemon Defender { get; set; }
        public Move Move { get; set; }

        public FightAction()
        {
            ActionType = ActionType.FIGHT;
        }

        public FightAction(Pokemon attacker, Pokemon defender, Move m)
        {
            this.Attacker = attacker;
            this.Defender = defender;
            this.Move = m;
        }

        public override Tuple<Move, Pokemon> GetMoveOrPokemon()
        {
            return new Tuple<Move, Pokemon>(Move, null);
        }

        public override string GetMessage()
        {
            string res = Attacker.Name + " uses " + Move.Name + ".";

            float effective = Battle.rules.GetTypeModifier(Move, Defender);
            if (effective > 1)
                res += System.Environment.NewLine + "It's super effective !";
            else if (effective < 1)
                res += System.Environment.NewLine + "It's not very effective...";

            return res;
        }
    }

    public class PokemonAction : BattleAction
    {
        public Trainer Trainer { get; set; }
        public Pokemon Pokemon { get; set; }

        public PokemonAction()
        {
            ActionType = ActionType.POKEMON;
        }

        public PokemonAction(Trainer t, Pokemon p)
        {
            this.Trainer = t;
            this.Pokemon = p;
        }

        public override Tuple<Move, Pokemon> GetMoveOrPokemon()
        {
            return new Tuple<Move, Pokemon>(null, Pokemon);
        }

        public override string GetMessage()
        {
            return Trainer.Name + " sends out " + Pokemon.Name + " !";
        }
    }
}