using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgram.Pages.Game {
    public class ListModel : PageModel {
        public string BackBtnHref { get; private set; } = "New";

        public bool HasAction { get; private set; }
        public bool HasId { get; private set; }

        public string Action { get; private set; }
        public string Id { get; private set; }

        public void OnGet() {
            HasAction = Request.Query.TryGetValue("action", out var action);
            HasId = Request.Query.TryGetValue("id", out var type);

            Action = action.ToString().ToLower().Trim();
            Id = type.ToString().ToLower().Trim();

            if (HasAction && HasId) {
                BackBtnHref = "Rules";
            }
        }
    }
}