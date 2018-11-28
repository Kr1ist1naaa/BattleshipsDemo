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

        public static readonly Rule BoardSize, PlayerCount, ShipCount, ShipPadding;
        
        static Rule() {
            BoardSize = new Rule {
                RuleName = "boardsize",
                Value = 10,
                AskOnInit = true
            };
            
            PlayerCount = new Rule {
                RuleName = "playercount",
                Value = 2,
                AskOnInit = true
            };
            
            ShipCount = new Rule {
                RuleName = "shipcount",
                Value = 5,
                AskOnInit = true
            };
            
            ShipPadding = new Rule {
                RuleName = "shippadding",
                Value = 1,
                AskOnInit = false
            };
        }
        
        public static List<Rule> GenBaseRuleSet() {
            return new List<Rule> {
                new Rule(BoardSize),
                new Rule(PlayerCount),
                new Rule(ShipCount),
                new Rule(ShipPadding)
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