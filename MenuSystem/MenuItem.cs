using System;
using System.Collections.Generic;
using Domain;

namespace MenuSystem {
    public class MenuItem {
        public string Shortcut { get; set; }
        public string Description { get; set; }
        public RuleType? RuleTypeToChange { get; set; }
        public bool IsDefaultChoice { get; set; }
        public bool IsResetRules { get; set; }
        public Func<string> MenuToRun { get; set; }
        public Action ActionToExecute { get; set; }
        public int? GameId { get; set; }

        public override string ToString() {
            return Shortcut == null ? Description : $"{Shortcut}) {Description}";
        }
    }
}