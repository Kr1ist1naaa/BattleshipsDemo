using System.Collections.Generic;
using System.Linq;
using Domain;
using GameSystem.Logic;

namespace GameSystem {
    public static class ActiveGame {
        public static readonly HashSet<Rule> RuleSet = RuleLogic.GenDefaultRuleSet();
        public static readonly List<Move> Moves = new List<Move>();
        public static List<Player> Players;
        public static Player Winner;
        public static int TurnCount;
        public static int? GameId = null;

        public static bool ChangeRule(RuleType? type, int ruleValue) {
            var baseRule = RuleSet.FirstOrDefault(m => m.RuleType.Equals(type));

            if (baseRule == null) {
                return false;
            }

            if (ruleValue > baseRule.MaxVal || ruleValue < baseRule.MinVal) {
                return false;
            }

            baseRule.Value = ruleValue;
            return true;
        }
        
        public static int GetRuleVal(RuleType? type) {
            return RuleSet.FirstOrDefault(m => m.RuleType.Equals(type)).Value;
        }

        public static Rule GetRule(RuleType? type) {
            return RuleSet.FirstOrDefault(m => m.RuleType.Equals(type));
        }

        public static void ResetGeneralRules() {
            RuleLogic.Reset(RuleSet, RuleInitializers.GeneralRules);
        }

        public static void ResetShipSizeRules() {
            RuleLogic.Reset(RuleSet, RuleInitializers.ShipSizeRules);
        }

        public static void ResetShipCountRules() {
            RuleLogic.Reset(RuleSet, RuleInitializers.ShipCountRules);
        }
        
        public static void ResetAllRules() {
            ResetGeneralRules();
            ResetShipSizeRules();
            ResetShipCountRules();
        }
    }
}