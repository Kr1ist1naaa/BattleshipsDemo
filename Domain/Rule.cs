using System.Collections.Generic;
using System.Linq;

namespace Domain {
    public class Rule {
        public string RuleName;
        public int Value;
        public bool Ask;

        public static int GetRule(IEnumerable<Rule> rules, Rule baseRule) {
            return (from rule in rules where rule.RuleName == baseRule.RuleName select rule.Value).FirstOrDefault();
        }

        public static readonly Rule BoardSize, PlayerCount, ShipCount, ShipsCanTouch;
        
        static Rule() {
            BoardSize = new Rule {
                RuleName = "boardsize",
                Value = 10,
                Ask = true
            };
            
            PlayerCount = new Rule {
                RuleName = "playercount",
                Value = 2,
                Ask = true
            };
            
            ShipCount = new Rule {
                RuleName = "shipcount",
                Value = 5,
                Ask = true
            };
            
            ShipsCanTouch = new Rule {
                RuleName = "shipscantouch",
                Value = 0,
                Ask = false
            };
        }
        
        public static List<Rule> GenBaseRuleSet() {
            return new List<Rule> {
                new Rule(BoardSize),
                new Rule(PlayerCount),
                new Rule(ShipCount),
                new Rule(ShipsCanTouch)
            };
        }
        
        public Rule(Rule rule) {
            RuleName = rule.RuleName;
            Value = rule.Value;
            Ask = rule.Ask;
        }
        
        private Rule() {}
    }
}