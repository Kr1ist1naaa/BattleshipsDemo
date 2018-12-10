using System.Collections.Generic;
using System.Linq;

namespace Domain {
    public class Rule {
        public string RuleName;
        public int Value;
        public bool AskOnInit;

        public static int GetRule(IEnumerable<Rule> rules, Rule baseRule) {
            return (from rule in rules where rule.RuleName == baseRule.RuleName select rule.Value).FirstOrDefault();
        }

        public static readonly Rule BoardSize, PlayerCount, ShipPadding;
        public static readonly Rule CarrierCount, BattleshipCount, SubmarineCount, CruiserCount, PatrolCount;
        public static readonly Rule CarrierSize, BattleshipSize, SubmarineSize, CruiserSize, PatrolSize;
        
        static Rule() {
            BoardSize = new Rule {
                RuleName = "default_size_board",
                Value = 10,
                AskOnInit = true
            };
            PlayerCount = new Rule {
                RuleName = "default_count_players",
                Value = 2,
                AskOnInit = true
            };
            ShipPadding = new Rule {
                RuleName = "default_ship_padding",
                Value = 1,
                AskOnInit = true
            };
            
            CarrierSize = new Rule {
                RuleName = "ship_size_carrier",
                Value = 5,
                AskOnInit = false
            };
            BattleshipSize = new Rule {
                RuleName = "ship_size_battleship",
                Value = 4,
                AskOnInit = false
            };
            SubmarineSize = new Rule {
                RuleName = "ship_size_submarine",
                Value = 3,
                AskOnInit = false
            };
            CruiserSize = new Rule {
                RuleName = "ship_size_cruiser",
                Value = 2,
                AskOnInit = false
            };
            PatrolSize = new Rule {
                RuleName = "ship_size_patrol",
                Value = 1,
                AskOnInit = false
            };
            
            CarrierCount = new Rule {
                RuleName = "ship_count_carrier",
                Value = 1,
                AskOnInit = false
            };
            BattleshipCount = new Rule {
                RuleName = "ship_count_battleship",
                Value = 1,
                AskOnInit = false
            };
            SubmarineCount = new Rule {
                RuleName = "ship_count_submarine",
                Value = 1,
                AskOnInit = false
            };
            CruiserCount = new Rule {
                RuleName = "ship_count_cruiser",
                Value = 1,
                AskOnInit = false
            };
            PatrolCount = new Rule {
                RuleName = "ship_count_patrol",
                Value = 1,
                AskOnInit = false
            };
        }
        
        public static List<Rule> GenBaseRuleSet() {
            return new List<Rule> {
                new Rule(BoardSize),
                new Rule(PlayerCount),
                new Rule(ShipPadding),
                new Rule(CarrierCount),
                new Rule(BattleshipCount),
                new Rule(SubmarineCount),
                new Rule(CruiserCount),
                new Rule(PatrolCount),
                new Rule(CarrierSize),
                new Rule(BattleshipSize),
                new Rule(SubmarineSize),
                new Rule(CruiserSize),
                new Rule(PatrolSize)
            };
        }
        
        private Rule(Rule rule) {
            RuleName = rule.RuleName;
            Value = rule.Value;
            AskOnInit = rule.AskOnInit;
        }
        
        private Rule() {}
    }
}