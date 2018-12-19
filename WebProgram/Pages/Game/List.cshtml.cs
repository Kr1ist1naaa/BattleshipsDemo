using Microsoft.AspNetCore.Mvc.RazorPages;
using SaveSystem;

namespace WebProgram.Pages.Game {
    public class ListModel : PageModel {
        public string Action { get; private set; }
        public string Id { get; private set; }
        
        public string OkBtnText { get; private set; } = "Ok";
        public string StatusMsg { get; private set; } = "[Status message]";
        public string BackBtnHref { get; private set; }
        public bool IsStatus { get; private set; }

        public void OnGet() {
            // Get action param
            if (!Request.Query.TryGetValue("action", out var action)) {
                return;
            }

            // Get id param
            if (!Request.Query.TryGetValue("id", out var id)) {
                return;
            }
            
            Action = action.ToString().ToLower().Trim();
            Id = id.ToString().ToLower().Trim();
            
            // Check params
            if (string.IsNullOrEmpty(Action) || string.IsNullOrEmpty(id) || !int.TryParse(id, out _)) {
                return;
            }

            if (Action.Equals("delete")) {
                GameSaver.Delete(int.Parse(Id));
                IsStatus = true;
                StatusMsg = "Game deleted!";
                BackBtnHref = "List";
            } else if (Action.Equals("load")) {
                GameSystem.GameLogic.Game = GameSaver.Load(int.Parse(Id));
                IsStatus = true;
                StatusMsg = "Game loaded!";
                OkBtnText = "Start game";
                BackBtnHref = "StartGame";
            }
        }
    }
}