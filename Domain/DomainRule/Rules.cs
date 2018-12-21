using System.Collections.Generic;
using System.Linq;

namespace Domain.DomainRule {
    public static class Rules {
        private static readonly HashSet<Rule> DefaultRuleSet = new HashSet<Rule> {
            new Rule {
                RuleType = RuleType.BoardSize,
                Description = "Set size of board",
                Value = 10,
                MinVal = 4,
                MaxVal = 256
            },
            new Rule {
                RuleType = RuleType.PlayerCount,
                Description = "Set player count",
                Value = 2,
                MinVal = 2,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.ShipPadding,
                Description = "Set ship padding",
                Value = 1,
                MinVal = 0,
                MaxVal = 6
            },
            new Rule {
                RuleType = RuleType.SizeCarrier,
                Description = "Set carrier size",
                Value = 5,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.SizeBattleship,
                Description = "Set battleship size",
                Value = 4,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.SizeSubmarine,
                Description = "Set submarine size",
                Value = 3,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.SizeCruiser,
                Description = "Set cruiser size",
                Value = 2,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.SizePatrol,
                Description = "Set patrol size",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.CountCarrier,
                Description = "Set carrier count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.CountBattleship,
                Description = "Set battleship count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.CountSubmarine,
                Description = "Set submarine count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.CountCruiser,
                Description = "Set cruiser count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            },
            new Rule {
                RuleType = RuleType.CountPatrol,
                Description = "Set patrol count",
                Value = 1,
                MinVal = 0,
                MaxVal = 64
            }
        };

        public static readonly HashSet<Rule> RuleSet = new HashSet<Rule>();

        static Rules() {
            // Create a set of base rules upon initialization 
            ResetAllToDefault();
        }

        private static void ResetRules(ICollection<RuleType> rules) {
            // Go through all default rules
            foreach (var rule in DefaultRuleSet) {
                // If the default rule matches the current one, replace it
                if (rules.Contains(rule.RuleType)) {
                    RuleSet.Remove(rule);
                    RuleSet.Add(new Rule(rule));
                }
            }
        }

        public static void ResetAllToDefault() {
            var removeRules = new HashSet<RuleType>();

            foreach (var rule in DefaultRuleSet) {
                removeRules.Add(rule.RuleType);
            }

            ResetRules(removeRules);
        }

        public static void ResetGeneralToDefault() {
            var removeRules = new HashSet<RuleType> {
                RuleType.BoardSize,
                RuleType.ShipPadding,
                RuleType.PlayerCount
            };

            ResetRules(removeRules);
        }

        public static void ResetShipSizesToDefault() {
            var removeRules = new HashSet<RuleType> {
                RuleType.SizeCarrier,
                RuleType.SizeBattleship,
                RuleType.SizeSubmarine,
                RuleType.SizeCruiser,
                RuleType.SizePatrol
            };

            ResetRules(removeRules);
        }

        public static void ResetShipCountsToDefault() {
            var removeRules = new HashSet<RuleType> {
                RuleType.CountCarrier,
                RuleType.CountBattleship,
                RuleType.CountSubmarine,
                RuleType.CountCruiser,
                RuleType.CountPatrol
            };

            ResetRules(removeRules);
        }

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

        public static int GetVal(RuleType? type) {
            return RuleSet.FirstOrDefault(m => m.RuleType.Equals(type)).Value;
        }

        public static Rule GetRule(RuleType? type) {
            return RuleSet.FirstOrDefault(m => m.RuleType.Equals(type));
        }
    }
}