using System.Collections.Generic;
using Domain;

namespace GameSystem.Logic {
    public static class RuleLogic {
        public static HashSet<Rule> GenDefaultRuleSet() {
            var rules = new HashSet<Rule>();

            foreach (var rule in RuleInitializers.DefaultRuleSet) {
                rules.Add(new Rule(rule));
            }

            return rules;
        }
        
        public static void Reset(HashSet<Rule> rules, HashSet<RuleType> ruleTypes) {
            // Go through all default rules
            foreach (var rule in RuleInitializers.DefaultRuleSet) {
                // If the default rule matches the current one, replace it
                if (ruleTypes.Contains(rule.RuleType)) {
                    rules.Remove(rule);
                    rules.Add(new Rule(rule));
                }
            }
        }
    }
}