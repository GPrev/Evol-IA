using PokeRules;
using System;

namespace PokeRules
{
    public enum ActionType { FIGHT, POKEMON, BAG, RUN, ANY, NONE }

    public delegate void OutDel(string str);

    public abstract class BattleAction : IEquatable<BattleAction>
    {
        public abstract ActionType GetActionType();

        /// <summary>
        /// Returns the chosen move or pokemon, the other being null.
        /// </summary>
        public abstract Tuple<Move, Pokemon> GetMoveOrPokemon();

        public Move GetMove() { return GetMoveOrPokemon().Item1; }
        public Pokemon GetPokemon() { return GetMoveOrPokemon().Item2; }

        public override bool Equals(Object o)
        {
            return Equals(o as BattleAction);
        }
        public bool Equals(BattleAction a)
        {
            if (a == null)
                return false;
            //else
            bool res = true;

            res &= (a.GetActionType() == GetActionType());

            if (a.GetMove() == null)
                res &= (GetMove() == null);
            else
                res &= (a.GetMove().Equals(GetMove()));

            if (a.GetPokemon() == null)
                res &= (GetPokemon() == null);
            else
                res &= (a.GetPokemon().Equals(GetPokemon()));

            return res;
        }

        public abstract bool CanBeExecuted();
        protected abstract void Execute(OutDel outD = null);
        public void SafeExecute(OutDel outD = null)
        {
            if (CanBeExecuted())
                Execute(outD);
        }
    }

    public class FightAction : BattleAction
    {
        public Pokemon Attacker { get; set; }
        public Trainer DefendingTrainer { get; set; }
        public Pokemon Defender { get { return DefendingTrainer.ActivePokemon; } }
        public Move Move { get; set; }

        public override ActionType GetActionType() { return ActionType.FIGHT; }

        public FightAction(Pokemon attacker, Trainer defender, Move m)
        {
            this.Attacker = attacker;
            this.DefendingTrainer = defender;
            this.Move = m;
        }

        public override Tuple<Move, Pokemon> GetMoveOrPokemon()
        {
            return new Tuple<Move, Pokemon>(Move, null);
        }

        public override bool CanBeExecuted()
        {
            return !Attacker.Ko() && !Defender.Ko();
        }

        protected override void Execute(OutDel outD = null)
        {
            if (outD != null)
                outD(Attacker.Name + " uses " + Move.Name + ".");

            int damage = Rules.ActiveRules.DamageFormula(Attacker, Defender, Move);
            Defender.CurrHP -= damage;

            if (damage > 0 && outD != null)
            {
                float effective = Rules.ActiveRules.GetTypeModifier(Move, Defender);
                if (effective > 1)
                    outD("It's super effective !");
                else if (effective < 1)
                    outD("It's not very effective...");

                if(Defender.Ko())
                    outD(Defender.Name + " fainted !");
            }
        }
    }

    public class PokemonAction : BattleAction
    {
        public Trainer Trainer { get; set; }
        public Pokemon Pokemon { get; set; }

        public override ActionType GetActionType() { return ActionType.POKEMON; }

        public PokemonAction(Trainer t, Pokemon p)
        {
            this.Trainer = t;
            this.Pokemon = p;
        }

        public override Tuple<Move, Pokemon> GetMoveOrPokemon()
        {
            return new Tuple<Move, Pokemon>(null, Pokemon);
        }

        public override bool CanBeExecuted()
        {
            return true;
        }

        protected override void Execute(OutDel outD = null)
        {
            if (outD != null)
                outD(Trainer.Name + " sends out " + Pokemon.Name + " !");
            Trainer.ActivePokemon = Pokemon;
        }
    }
}