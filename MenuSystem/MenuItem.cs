using System;
using Domain.Rule;

namespace MenuSystem {
    public class MenuItem {
        public string Shortcut { get; set; }
        public string Description { get; set; }
        public RuleType RuleType { get; set; }

        public Func<string> CommandToExecute { get; set; }

        public bool IsDefaultChoice { get; set; }

        public override string ToString() {
            return Shortcut + ") " + Description;
        }
    }
}