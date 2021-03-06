﻿using System;

namespace PokeRules
{
    public enum ActionType { FIGHT, POKEMON, BAG, RUN, ANY, NONE }

    public delegate void OutDel(string str);

    public abstract class BattleAction : IEquatable<BattleAction>
    {
        public abstract ActionType GetActionType();

        public override bool Equals(Object o)
        {
            return Equals(o as BattleAction);
        }
        public virtual bool Equals(BattleAction a)
        {
            if (a == null)
                return false;
            //else
            return (a.GetActionType() == GetActionType());
        }

        public abstract int GetActorId();

        public abstract bool CanBeExecuted(BattleState s);
        protected abstract void Execute(BattleState s, OutDel outD = null);
        public void SafeExecute(BattleState s, OutDel outD = null)
        {
            if (CanBeExecuted(s))
                Execute(s, outD);
        }
    }

    public class FightAction : BattleAction
    {
        public int attackerID;
        public int defenderID;
        public Move Move { get; set; }

        public override ActionType GetActionType() { return ActionType.FIGHT; }

        public FightAction(int attacker, int defender, Move m)
        {
            this.attackerID = attacker;
            this.defenderID = defender;
            this.Move = m;
        }

        public override bool CanBeExecuted(BattleState s)
        {
            if(Move.TargetSelf)
                return !getAttacker(s).Ko() && !s.Trainers[defenderID].IsOutOfPokemon();
            //else
            return !getAttacker(s).Ko() && !getDefender(s).Ko();
        }

        protected override void Execute(BattleState s, OutDel outD = null)
        {
            Pokemon attacker = getAttacker(s);
            Pokemon target;
            if (Move.TargetSelf)
                target = attacker;
            else
                target = getDefender(s);

            if (outD != null)
                outD(attacker.Name + " uses " + Move.Name + ".");

            int damage = Rules.ActiveRules.DamageFormula(attacker, target, Move);
            target.CurrHP -= damage;

            if (damage > 0 && outD != null)
            {
                float effective = Rules.ActiveRules.GetTypeModifier(Move, target);
                if (effective > 1)
                    outD("It's super effective !");
                else if (effective < 1)
                    outD("It's not very effective...");

                if(target.Ko())
                    outD(target.Name + " fainted !");
            }

            if (!target.Ko())
            {
                // Stats alterations
                if(Move.AlteredStat != Statistic.NONE && Move.StatModifier != 0)
                {
                    int oldModifier = target.GetStatModifier(Move.AlteredStat);
                    target.AddStatModifier(Move.AlteredStat, Move.StatModifier);
                    int realModification = target.GetStatModifier(Move.AlteredStat) - oldModifier;

                    if (outD != null)
                        outD(getStatModMessage(target, Move.AlteredStat, realModification));
                }

                // Condition status
                if (Move.Condition != Condition.OK && Rules.ActiveRules.CanApplyCondition(Move.Condition, target))
                {
                    target.Condition = Move.Condition;
                    if(outD != null)
                        outD(GetConditionMessage(Move.Condition, target));
                }
            }
        }

        private string getStatModMessage(Pokemon pokemon, Statistic alteredStat, int realModification)
        {
            if (realModification == 0)
                return "Nothing happened !";
            //else
            string res = pokemon.Name + "'s " + alteredStat.ToString().ToLower();

            if (realModification > 1 || realModification < -1)
                res += " greatly";

            if (realModification > 0)
                res += " rose !";
            else if (realModification < 0)
                res += " fell !";

            return res;
        }

        private string GetConditionMessage(Condition condition, Pokemon pokemon)
        {
            string res = "";

            switch(condition)
            {
                case Condition.POISONED:
                    res = pokemon.Name + " was poisoned !";
                    break;
            }

            return res;
        }

        public Pokemon getAttacker(BattleState s)
        {
            return s.Trainers[attackerID].ActivePokemon;
        }

        public Pokemon getDefender(BattleState s)
        {
            return s.Trainers[defenderID].ActivePokemon;
        }

        public override bool Equals(BattleAction a)
        {
            if (!base.Equals(a))
                return false;
            //else
            FightAction aa = a as FightAction;
            if (aa == null)
                return false;
            //else
            return attackerID == aa.attackerID && defenderID == aa.defenderID && Move.Equals(aa.Move);
        }

        public override int GetActorId()
        {
            return attackerID;
        }
    }

    public class PokemonAction : BattleAction
    {
        public int trainerID;
        public int pokemonID;

        public override ActionType GetActionType() { return ActionType.POKEMON; }

        public PokemonAction(int trainer, int pokemon)
        {
            this.trainerID = trainer;
            this.pokemonID = pokemon;
        }

        public override bool CanBeExecuted(BattleState s)
        {
            return true;
        }

        protected override void Execute(BattleState s, OutDel outD = null)
        {
            Pokemon oldPokemon = getTrainer(s).ActivePokemon;

            if (outD != null)
                outD(getTrainer(s).Name + " sends out " + getPokemon(s).Name + " !");
            getTrainer(s).ActivePokemon = getPokemon(s);

            oldPokemon.ResetStatModifiers();
        }

        public Trainer getTrainer(BattleState s)
        {
            return s.Trainers[trainerID];
        }

        public Pokemon getPokemon(BattleState s)
        {
            return getTrainer(s).Team[pokemonID];
        }

        public override bool Equals(BattleAction a)
        {
            if (!base.Equals(a))
                return false;
            //else
            PokemonAction aa = a as PokemonAction;
            if (aa == null)
                return false;
            //else
            return trainerID == aa.trainerID && pokemonID == aa.pokemonID;
        }

        public override int GetActorId()
        {
            return trainerID;
        }
    }
}