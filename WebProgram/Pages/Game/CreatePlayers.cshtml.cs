using System.Collections.Generic;
using GameSystem;
using GameSystem.Logic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Player = Domain.Player;

namespace WebProgram.Pages.Game {
    public class CreatePlayersModel : PageModel {
        public bool IsStatus { get; private set; }
        public bool IsError { get; private set; }
        public bool IsInvalidAccess { get; private set; }
        public string StatusMsg { get; private set; }

        public void OnGet() {
            if (ActiveGame.RuleSet == null) {
                IsInvalidAccess = true;
                return;
            }
            
            // Reset current players
            ActiveGame.Players = null;
        }

        public void OnPost() {
            IsStatus = true;
            ActiveGame.Players = new List<Player>();
            
            // Reset last game state
            ActiveGame.Init();
            
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
                if (!InputValidator.CheckValidPlayerName(name)) {
                    IsError = true;
                    StatusMsg = "Invalid player name!";
                    return;
                }

                ActiveGame.Players.Add(new Player(name, ShipLogic.GenGameShipList()));
            }
            
            // Set pointers
            ActiveGame.InitPlayerPointers();

            StatusMsg = "Players created!";
        }
    }
}