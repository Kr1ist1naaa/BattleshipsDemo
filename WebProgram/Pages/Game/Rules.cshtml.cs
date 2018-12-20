using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgram.Pages.Game {
    public class RulesModel : PageModel {
        public string MainTitle { get; } = "Rules for new game";
        public string BackBtnHref { get; } = "/Game/Create";
        
        public string StatusMsg { get; private set; } = "[Status message]";
        public bool IsStatus { get; private set; }
       
        public void OnPost() {
            IsStatus = true;
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
                GameSystem.ActiveGame.ChangeRule((RuleType) intKey, intVal);
            }
        }

        public void OnGet() {
            if (Request.Query.ContainsKey("reset")) {
                IsStatus = true;
                StatusMsg = "Rules reverted to default values!";
                GameSystem.ActiveGame.ResetAllRules();
            }
        }
    }
}