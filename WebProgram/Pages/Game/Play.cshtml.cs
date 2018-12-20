using System;
using System.Linq;
using Domain;
using GameSystem;
using GameSystem.Logic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaveSystem;

namespace WebProgram.Pages.Game {
    public class PlayModel : PageModel {
        public bool IsInvalidAccess { get; private set; }
        public bool IsStatus { get; private set; }
        public bool IsError { get; private set; }
        public bool IsWinner { get; private set; }
        public bool IsDisplaySave { get; private set; }
        public bool IsDisplayBoards { get; private set; }
        public string StatusMsg { get; private set; }
        public string MainTitle { get; private set; } = "Attack menu";

        public void OnGet() {
            // Players have not been defined yet
            if (ActiveGame.Players == null || ActiveGame.Players.Count == 0) {
                IsInvalidAccess = true;
                return;
            }
            
            // There's no current player so set it
            if (ActiveGame.CurrentPlayer == null || ActiveGame.NextPlayer == null) {
                ActiveGame.InitPlayerPointers();
                
                // Just to get rid of IDE warnings
                if (ActiveGame.CurrentPlayer == null || ActiveGame.NextPlayer == null) {
                    throw new NullReferenceException();
                }
            }
            
            // Players haven't placed ships yet
            if (ActiveGame.CurrentPlayer.Ships.FirstOrDefault(ship => ship.IsPlaced) == null) {
                IsInvalidAccess = true;
                return;
            }

            if (Request.Query.ContainsKey("save")) {
                GameSaver.Save();
                IsStatus = true;
                StatusMsg = "Game saved!";
                return;
            }
            
            // There's a winner
            if (ActiveGame.TrySetWinner()) {
                IsWinner = true;
                StatusMsg = $"The winner of the game is {ActiveGame.Winner.Name}!";
                return;
            }

            MainTitle = $"{ActiveGame.CurrentPlayer.Name} is attacking {ActiveGame.NextPlayer.Name}";
            IsDisplayBoards = true;
        }

        public void OnPost() {
            if (!Request.Form.TryGetValue("x", out var strX)) {
                IsError = true;
                StatusMsg = "Missing x coordinate!";
                return;
            }
            
            if (!Request.Form.TryGetValue("y", out var strY)) {
                IsError = true;
                StatusMsg = "Missing y coordinate!";
                return;
            }

            // Check if the specified location can be attacked
            if (!InputValidator.CheckValidAttackLocation(ActiveGame.NextPlayer, strX, strY, out var pos)) {
                IsError = true;
                StatusMsg = "Invalid attack location!";
                return;
            }
            
            // Make the attack
            var result = PlayerLogic.AttackPlayer(ActiveGame.NextPlayer, pos);

            // Return whatever
            var move = new Move(ActiveGame.CurrentPlayer, ActiveGame.NextPlayer, pos, result);

            // Get next player
            var lastTurn = ActiveGame.RoundCounter;
            ActiveGame.CyclePlayers();

            if (lastTurn != ActiveGame.RoundCounter) {
                IsDisplaySave = true;
            }
            
            IsStatus = true;
            StatusMsg = $"It was a {result}!";
        }

        public string GetRoundClass(int size, int x, int y) {
            if (y == 0 && x == 0) {
                return "rounded-top-left";
            } 
            
            if (y == size - 1 && x == 0) {
                return "rounded-bottom-left";
            } 
            
            if (y == 0 && x == size - 1) {
                return "rounded-top-right";
            } 
            
            if (y == size - 1 && x == size - 1) {
                return "rounded-bottom-right";
            }

            return "";
        }
    }
}