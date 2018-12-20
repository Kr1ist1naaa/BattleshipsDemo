using System.Linq;
using System.Text.RegularExpressions;
using Domain;
using GameSystem.Logic;

namespace GameSystem {
    public static class InputValidator {
        public static bool CheckValidShipPlacementLoc(Domain.Player player, int shipSize, string strX, string strY,
            string strDir, out Pos pos, out ShipDirection dir) {
            pos = null;
            dir = ShipDirection.Down;

            if (strDir.Equals("r") || strDir.Equals("right")) {
                dir = ShipDirection.Right;
            } else if (strDir.Equals("d") || strDir.Equals("down")) {
                dir = ShipDirection.Down;
            } else {
                return false;
            }

            // Check if x coordinate is valid and alphabetic
            if (BaseConversion.MapToBase10(strX) == null) {
                return false;
            }

            // Check if y coordinate is a valid integer
            if (!int.TryParse(strY, out var intY)) {
                return false;
            }

            // Convert X coordinate to an integer
            var intX = (int) BaseConversion.MapToBase10(strX);

            // Take board orientation and numbering offset into account
            var x = intX;
            var y = intY - 1;
            
            pos = new Pos(x, y);

            return PlayerLogic.CheckValidShipPlacementPos(player, pos, shipSize, dir);
        }
        
        public static bool CheckValidPlayerName(string name) {
            // Check input validity
            if (string.IsNullOrEmpty(name) || name.Trim().Length < 3) {
                return false;
            }
            
            // Only accept alphanumeric names
            if (!new Regex("^[a-zA-Z0-9]*$").IsMatch(name)) {
                return false;
            }

            // Check name availability
            if (ActiveGame.Players?.FirstOrDefault(m => m.Name.Equals(name)) != null) {
                return false;
            }

            return true;
        }

        public static bool CheckValidAttackLocation(Player player, string strX, string strY, out Pos pos) {
            pos = null;
            
            // Check if x coordinate is valid and alphabetic
            if (BaseConversion.MapToBase10(strX) == null) {
                return false;
            }

            // Check if y coordinate is a valid integer
            if (!int.TryParse(strY, out var intY)) {
                return false;
            }

            // Convert X coordinate to an integer
            var intX = (int) BaseConversion.MapToBase10(strX);
            pos = new Pos(intX, intY - 1);
            
            // Out of bounds
            if (!PlayerLogic.CheckIfPosInBoard(pos)) {
                return false;
            }

            // Duplicate attack
            if (player.MovesAgainstThisPlayer.Contains(pos)) {
                return false;
            }

            // Location is within board and has not been attacked before
            return true;
        }
    }
}