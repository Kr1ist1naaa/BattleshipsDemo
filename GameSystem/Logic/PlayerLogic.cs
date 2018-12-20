using Domain;

namespace GameSystem.Logic {
    public static class PlayerLogic {
        public static bool CheckValidShipPlacementPos(Player player, Pos pos, int shipSize, ShipDirection direction) {
            // Check if position is off board
            if (!CheckIfPosInBoard(pos)) {
                return false;
            }

            // Find ship's furthest point
            var maxPos = new Pos(
                pos.X + (direction == ShipDirection.Right ? shipSize - 1 : 0),
                pos.Y + (direction == ShipDirection.Right ? 0 : shipSize - 1)
            );

            // Check if max position is off board
            if (!CheckIfPosInBoard(maxPos)) {
                return false;
            }

            // Check if any ships already exist at that location
            foreach (var ship in player.Ships) {
                if (ShipLogic.CheckIfIntersect(ship, pos, shipSize, direction)) {
                    return false;
                }
            }

            return true;
        }
        
        public static bool CheckIfPosInBoard(Pos pos) {
            var boardSize = ActiveGame.GetRuleVal(RuleType.BoardSize);

            // Out of bounds
            if (pos.X < 0 || pos.X >= boardSize) return false;
            if (pos.Y < 0 || pos.Y >= boardSize) return false;

            return true;
        }

        public static void ResetShips(Player player) {
            player.Ships = ShipLogic.GenGameShipList();
        }

        public static Ship GetShipOrNull(Player player, Pos pos) {
            if (player.Ships == null) {
                return null;
            }
            
            foreach (var ship in player.Ships) {
                if (ShipLogic.IsAtPos(ship, pos)) {
                    return ship;
                }
            }

            return null;
        }
        
        public static AttackResult AttackPlayer(Player player, Pos pos) {
            player.MovesAgainstThisPlayer.Add(pos);
            var ship = GetShipOrNull(player, pos);
            return ShipLogic.AttackAtPos(ship, pos);
        }
        
        public static bool IsAlive(Player player) {
            // Count each ship block individually, checking if there is at least one that's not hit
            foreach (var ship in player.Ships) {
                if (!ShipLogic.IsDestroyed(ship)) {
                    return true;
                }
            }

            return false;
        }

        public static bool IsPlacedAllShips(Player player) {
            foreach (var ship in player.Ships) {
                if (!ship.IsPlaced) {
                    return false;
                }
            }

            return true;
        }
    }
}