using System.Collections.Generic;
using Domain;
using GameSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgram.Pages.Game {
    public class CreatePlayersModel : PageModel {
        public bool IsStatus { get; private set; }
        public bool IsError { get; private set; }
        public string StatusMsg { get; private set; }

        public void OnGet() {
        }

        public void OnPost() {
            IsStatus = true;
            GameLogic.Players = new List<Player>();
            
            // Loop through each form param
            foreach (var key in Request.Form.Keys) {
                // Only check names
                if (!key.Contains("name-")) {
                    continue;
                }
                    
                // Get name
                Request.Form.TryGetValue(key, out var names);
                var name = names.ToString();
                
                // Check input validity
                if (!InputValidator.ValidatePlayerName(name)) {
                    IsError = true;
                    StatusMsg = "Invalid player name!";
                    return;
                }

                GameLogic.Players.Add(new Player(name));
            }

            StatusMsg = "Players created!";
        }
    }
}