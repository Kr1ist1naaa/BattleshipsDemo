using System.Collections.Generic;

namespace Domain {
    public static class RuleInitializers {
        public static readonly HashSet<Rule> DefaultRuleSet = new HashSet<Rule> {
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
        
        public static readonly HashSet<RuleType> GeneralRules = new HashSet<RuleType> {
            RuleType.BoardSize,
            RuleType.ShipPadding,
            RuleType.PlayerCount
        };
        
        public static readonly HashSet<RuleType> ShipSizeRules = new HashSet<RuleType> {
            RuleType.SizeCarrier,
            RuleType.SizeBattleship,
            RuleType.SizeSubmarine,
            RuleType.SizeCruiser,
            RuleType.SizePatrol
        };
        
        public static readonly HashSet<RuleType> ShipCountRules = new HashSet<RuleType> {
            RuleType.CountCarrier,
            RuleType.CountBattleship,
            RuleType.CountSubmarine,
            RuleType.CountCruiser,
            RuleType.CountPatrol
        };
    }
}