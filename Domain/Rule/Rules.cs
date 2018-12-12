using System.Collections.Generic;
using System.Linq;

namespace Domain.Rule {
    public static class Rules {
        private static readonly HashSet<BaseRule> DefaultRuleSet = new HashSet<BaseRule> {
            new BaseRule {
                RuleType = RuleType.BoardSize,
                Description = "Size of board",
                Value = 10,
                MinVal = 4,
                MaxVal = 256
            },
            new BaseRule {
                RuleType = RuleType.PlayerCount,
                Description = "Player count",
                Value = 2,
                MinVal = 2,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.ShipPadding,
                Description = "Amount of blocks to pad around ships",
                Value = 1,
                MinVal = 0,
                MaxVal = 6
            },
            new BaseRule {
                RuleType = RuleType.SizeCarrier,
                Description = "Carrier size",
                Value = 5,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.SizeBattleship,
                Description = "Battleship size",
                Value = 4,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.SizeSubmarine,
                Description = "Submarine size",
                Value = 3,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.SizeCruiser,
                Description = "Cruiser size",
                Value = 2,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.SizePatrol,
                Description = "Patrol size",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.CountCarrier,
                Description = "Carrier count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.CountBattleship,
                Description = "Battleship count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.CountSubmarine,
                Description = "Submarine count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.CountCruiser,
                Description = "Cruiser count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new BaseRule {
                RuleType = RuleType.CountPatrol,
                Description = "Patrol count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            }
        };
        public static readonly HashSet<BaseRule> RuleSet = new HashSet<BaseRule>();

        static Rules() {
            // Create a set of base rules upon initialization 
            ResetDefault();
        }

        public static void ResetDefault() {
            RuleSet.Clear();
            
            // Recreate base rules based on the default rule set
            foreach (var rule in DefaultRuleSet) {
                RuleSet.Add(new BaseRule(rule));
            }
        }
        
        public static bool ChangeRule(RuleType type, int ruleValue) {
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

        public static int GetVal(RuleType type) {
            return RuleSet.FirstOrDefault(m => m.RuleType.Equals(type)).Value;
        }
        
        public static BaseRule GetRule(RuleType type) {
            return RuleSet.FirstOrDefault(m => m.RuleType.Equals(type));
        }
    }
}