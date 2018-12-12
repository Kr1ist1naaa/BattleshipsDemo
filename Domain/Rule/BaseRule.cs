
namespace Domain.Rule {
    public class BaseRule {
        public string Description;
        public int Value, MinVal, MaxVal;
        public RuleType RuleType;
        
        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            if (typeof(BaseRule) != obj.GetType()) {
                return false;
            }

            var other = (BaseRule) obj;

            if (RuleType != other.RuleType) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() {
            var hash = 3;

            if (RuleType != null) hash = 53 * hash + RuleType.GetHashCode();

            return hash;
        }

        public BaseRule(BaseRule oldRule) {
            Description = oldRule.Description;
            Value = oldRule.Value;
            MinVal = oldRule.MinVal;
            MaxVal = oldRule.MaxVal;
            RuleType = oldRule.RuleType;
        }
        
        public BaseRule() {}
    }
}