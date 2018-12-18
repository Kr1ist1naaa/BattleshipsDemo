using Domain.DomainRule;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgram.Pages.Game {
    public class RulesModel : PageModel {
        public string MainTitle { get; } = "Rules for new game";
        public string BackBtnHref { get; } = "/Game/New";
        public string StatusMsg { get; private set; } = "[Status message]";

        public bool IsAction { get; private set; }
       
        public void OnPost() {
            IsAction = true;
            StatusMsg = "Rules saved!";

            foreach (var key in Request.Form.Keys) {
                // Skip empty parameters
                if (!Request.Form.TryGetValue(key, out var strVal)) {
                    continue;
                }

                // Skip non-integer parameters
                if (!int.TryParse(key, out var intKey)) continue;
                if (!int.TryParse(strVal, out var intVal)) continue;

                // Update static rule context
                Rules.ChangeRule((RuleType) intKey, intVal);
            }
        }

        public void OnGet() {
            if (Request.Query.ContainsKey("reset")) {
                IsAction = true;
                Rules.ResetAllToDefault();
                StatusMsg = "Rules reverted to default values!";
            }
        }
    }
}