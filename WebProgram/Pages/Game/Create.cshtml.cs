using GameSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgram.Pages.Game {
    public class CreateModel : PageModel {
        public void OnGet() {
            // Reset current rules
            ActiveGame.ResetAllRules();
        }
    }
}