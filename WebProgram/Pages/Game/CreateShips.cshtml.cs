using Domain;
using Domain.DomainShip;
using GameSystem;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebProgram.Pages.Game {
    public class CreateShipsModel : PageModel {
        public string MainTitle { get; private set; }

        public bool IsAutoplaceQuestion { get; private set; }
        public bool IsCurrentPlayerPlacedAll { get; private set; }
        public bool IsAllPlayerPlacedAll { get; private set; }
        public string BackBtnHref { get; private set; }

        public bool IsStatus { get; private set; }
        public bool IsDisplayBoard { get; private set; }
        public bool IsError { get; private set; }
        public string StatusMsg { get; private set; }

        public int PlayerId { get; private set; }
        public int ShipId { get; private set; }
        public int NextPlayerId { get; private set; }
        public int NextShipId { get; private set; }

        public Player Player { get; private set; }
        public Ship Ship { get; private set; }

        public void OnGet() {
            // Get, parse and check player id query param
            if (!GetPlayer()) {
                return;
            }

            if (Request.Query.ContainsKey("askAutoPlace")) {
                Player.ResetShips();
                IsAutoplaceQuestion = true;
                MainTitle = $"Creating ships for {Player.Name}";
                return;
            }

            // Missing placement param
            if (!Request.Query.TryGetValue("placement", out var placement)) {
                IsError = true;
                StatusMsg = "Missing placement param!";
                BackBtnHref = $"?player={PlayerId}&askAutoPlace";
                return;
            }

            // Unknown placement param 
            if (!placement.Equals("auto") && !placement.Equals("manual")) {
                IsError = true;
                StatusMsg = "Unknown placement param!";
                BackBtnHref = $"?player={PlayerId}&askAutoPlace";
                return;
            }

            // Attempt to place the ships automatically
            if (placement.Equals("auto")) {
                Player.ResetShips();
                
                if (GameLogic.AutoPlaceShips(Player)) {
                    IsCurrentPlayerPlacedAll = true;
                    IsDisplayBoard = true;
                } else {
                    IsError = true;
                    StatusMsg = "Could not place ships!";
                    BackBtnHref = $"?player={PlayerId}&askAutoPlace";
                }

                return;
            }

            // Place ships manually
            if (placement.Equals("manual")) {
                // Get, parse and check ship id query param
                if (!GetShip()) return;
                
                IsDisplayBoard = true;
                MainTitle = $"Creating ship {ShipId + 1}/{Player.Ships.Count} for {Player.Name}";
            }
        }

        public void OnPost() {
            IsStatus = true;

            // Get, parse and check player id query param
            if (!GetPlayer()) {
                return;
            }
            
            // Get, parse and check ship id query param
            if (!GetShip()) {
                return;
            }

            // Get, parse and check post form data
            if (!ParsePostForm(out var sX, out var sY, out var sDir)) {
                BackBtnHref = $"?player={PlayerId}&placement=manual&ship={ShipId}";
                IsError = true;
                return;
            }

            // Check if it's a valid placement spot
            if (!InputValidator.ShipPlacementLoc(Player, Ship.Size, sX, sY, sDir, out var pos, out var dir)) {
                IsError = true;
                StatusMsg = "Invalid location";
                BackBtnHref = $"?player={PlayerId}&placement=manual&ship={ShipId}";
                return;
            }

            Ship.SetLocation(pos, dir);
            Ship.IsPlaced = true;
            
            StatusMsg = $"{Ship.Type} placed successfully!";
            BackBtnHref = $"?player={PlayerId}&placement=manual&ship={ShipId + 1}";
            IsDisplayBoard = true;
        }

        private bool GetPlayer() {
            if (!Request.Query.TryGetValue("player", out var strPlayerId) || !int.TryParse(strPlayerId, out var pId) || pId < 0) {
                IsError = true;
                StatusMsg = "Invalid player param!";
                BackBtnHref = "?player=0&askAutoPlace";
                return false;
            }

            if (pId >= GameLogic.Players.Count) {
                // Check if all users have placed their ships
                if (!CheckIfShipsPlaced(GameLogic.Players.Count)) {
                    IsError = true;
                    StatusMsg = "Invalid player order 2!";
                    BackBtnHref = "?player=0&askAutoPlace";
                    return false;
                }
                
                IsAllPlayerPlacedAll = true;
                return false;
            }
            
            PlayerId = pId;
            Player = GameLogic.Players[PlayerId];
            NextPlayerId = PlayerId + 1;

            // Check if all previous users placed their ships
            if (!CheckIfShipsPlaced(PlayerId)) {
                IsError = true;
                StatusMsg = "Invalid player order!";
                BackBtnHref = "?player=0&askAutoPlace";
                return false;
            }

            return true;
        }

        private static bool CheckIfShipsPlaced(int id) {
            // Check if all previous users placed their ships
            for (int j = 0; j < id; j++) {
                foreach (var ship in GameLogic.Players[j].Ships) {
                    if (!ship.IsPlaced) {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool GetShip() {
            if (!Request.Query.TryGetValue("ship", out var strShip) || !int.TryParse(strShip, out var shipId) ||
                shipId < 0 || shipId >= Player.Ships.Count) {
                    
                IsError = true;
                StatusMsg = "Invalid ship param!";
                BackBtnHref = $"?player={PlayerId}&askAutoPlace";
                return false;
            }

            ShipId = shipId;
            Ship = Player.Ships[ShipId];

            // Check if all previous ships were placed
            for (int i = 0; i < ShipId; i++) {
                if (!Player.Ships[i].IsPlaced) {
                    IsError = true;
                    StatusMsg = "Invalid ship order!";
                    BackBtnHref = $"?player={PlayerId}&askAutoPlace";
                    return false;
                }
            }

            // If this ship has already been placed
            if (Ship.IsPlaced) {
                IsError = true;
                StatusMsg = "Invalid ship!";
                BackBtnHref = $"?player={PlayerId}&askAutoPlace";
                return false;
            }

            return true;
        }

        private bool ParsePostForm(out string x, out string y, out string dir) {
            x = y = dir = null;
            
            if (!Request.Form.TryGetValue("x", out var strX)) {
                StatusMsg = "No x coordinate found!";
                return false;
            }

            if (!Request.Form.TryGetValue("y", out var strY)) {
                StatusMsg = "No y coordinate found!";
                return false;
            }

            if (!Request.Form.TryGetValue("direction", out var strDir)) {
                StatusMsg = "No direction found!";
                return false;
            }

            x = strX;
            y = strY;
            dir = strDir;
            return true;
        }
    }
}