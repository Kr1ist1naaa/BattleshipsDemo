using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgram.Pages.Game {
    public class RulesModel : PageModel {
        public string MainTitle { get; private set; } = "Changing rules for new game";
        public string ResetBtnTitle { get; private set; } = "Reset all rules";
        public string BackBtnHref { get; private set; } = "New";

        public bool HasAction { get; private set; }
        public bool HasType { get; private set; }
        public bool GenRulesTable { get; private set; }
        public bool IsReset { get; private set; }
        public bool IsSave { get; private set; }

        public string Action { get; private set; }
        public string Type { get; private set; }

        public void OnGet() {
            HasAction = Request.Query.TryGetValue("action", out var action);
            HasType = Request.Query.TryGetValue("type", out var type);

            Action = action.ToString().ToLower().Trim();
            Type = type.ToString().ToLower().Trim();

            if (HasAction && HasType) {
                BackBtnHref = "Rules";
                
                MainTitle = $"Changing {Type} rules";
                ResetBtnTitle = $"Reset {Type} rules";
                    
                if (Action.Equals("list")) {
                    if (Type.Equals("general") || Type.Equals("size") || Type.Equals("count")) {
                        GenRulesTable = true;
                    } 
                }
                
                IsReset = Action.Equals("reset");
                IsSave = Action.Equals("save");
                    
                if ((IsReset || IsSave) && !Type.Equals("all")) {
                    BackBtnHref = $"Rules?action=list&type={Type}";
                }
            }
        }
    }
}