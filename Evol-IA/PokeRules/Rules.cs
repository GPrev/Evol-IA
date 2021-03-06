﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeRules
{
    public abstract class Rules
    {
        public static Rules ActiveRules = new G4Rules();
        
        public abstract int DamageFormula(Pokemon attP, Pokemon defP, Move m);

        public abstract float GetTypeModifier(Type attackType, Type defType1, Type defType2 = Type.NONE);

        public float GetTypeModifier(Move m, Pokemon p)
        {
            return GetTypeModifier(m.Type, p.Type, p.Type2);
        }

        public abstract int FasterThan(BattleState s, FightAction a1, FightAction a2);

        public abstract int FasterThan(ActionType a1, ActionType a2);

        public int FasterThan(BattleState s, BattleAction a1, BattleAction a2)
        {
            int res = FasterThan(a1.GetActionType(), a2.GetActionType());
            if (res != 0)
                return res;
            //else
            if (a1.GetActionType() == ActionType.FIGHT)
                return FasterThan(s, a1 as FightAction, a2 as FightAction);
            //else
            return 0; // Nonimportant
        }

        public void OrderActions(BattleState s, List<BattleAction> actions)
        {
            actions.RemoveAll(IsNull);
            actions.Sort((a1, a2) => { return FasterThan(s, a1, a2); });
        }

        private static bool IsNull(BattleAction obj)
        {
            return obj == null;
        }

        public abstract bool CanApplyCondition(Condition condition, Pokemon pokemon);
    }
}
