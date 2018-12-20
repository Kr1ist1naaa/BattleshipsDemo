using Domain;
using GameSystem;
using GameSystem.Logic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgram.Pages.Game {
    public class RulesModel : PageModel {
        public string StatusMsg { get; private set; } = "[Status message]";
        public bool IsStatus { get; private set; }
        public bool IsError { get; private set; }
        public bool IsDisplayContinue { get; private set; }
       
        public void OnPost() {
            // Overwrite current rules
            foreach (var key in Request.Form.Keys) {
                // Get value for current key
                Request.Form.TryGetValue(key, out var strVal);
                
                // Skip empty fields
                if (string.IsNullOrWhiteSpace(strVal) || string.IsNullOrEmpty(strVal)) {
                    continue;
                }

                // Skip non-integer parameters
                if (!int.TryParse(key, out var intKey)) {
                    continue;
                }

                if (!int.TryParse(strVal, out var intVal)) {
                    StatusMsg = "Non-integer rules!";
                    IsError = true;
                    return;
                }

                // Update static rule context
                ActiveGame.TryChangeRule((RuleType) intKey, intVal);
            }

            IsStatus = true;
            StatusMsg = "Rules saved!";
            IsDisplayContinue = true;
        }

        public void OnGet() {
            if (!Request.Query.ContainsKey("persist")) {
                ActiveGame.ResetAllRules();
            }
            
            if (Request.Query.ContainsKey("reset")) {
                IsStatus = true;
                StatusMsg = "Rules reverted to default values!";
                IsDisplayContinue = true;
            }
        }
    }
}