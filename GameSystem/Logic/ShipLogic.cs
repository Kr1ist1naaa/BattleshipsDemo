using System.Collections.Generic;
using System.Linq;
using Domain;

namespace GameSystem.Logic {
    public static class ShipLogic {
        public static bool CheckIfIntersect(Ship ship, Pos newPos, int newSize, ShipDirection newDirection) {
            // Ship hasn't been placed yet
            if (ship.ShipPos == null) {
                return false;
            }

            var padding = ActiveGame.GetRuleVal(RuleType.ShipPadding);

            // This is one retarded way of doing it, but hey, it works...
            for (int offset = 0; offset < newSize; offset++) {
                var newPosOffsetX = newPos.X + (newDirection == ShipDirection.Right ? offset : 0);
                var newPosOffsetY = newPos.Y + (newDirection == ShipDirection.Right ? 0 : offset);

                for (int i = 0; i < ship.Size; i++) {
                    var shipPosX = ship.ShipPos.X + (ship.Direction == ShipDirection.Right ? i : 0);
                    var shipPosY = ship.ShipPos.Y + (ship.Direction == ShipDirection.Right ? 0 : i);

                    if (shipPosX >= newPosOffsetX - padding && shipPosX <= newPosOffsetX + padding &&
                        shipPosY >= newPosOffsetY - padding && shipPosY <= newPosOffsetY + padding) {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsAtPos(Ship ship, Pos pos) {
            // Ship hasn't been placed yet
            if (ship.ShipPos == null) {
                return false;
            }

            for (int i = 0; i < ship.Size; i++) {
                var shipBlockPosX = ship.ShipPos.X + (ship.Direction == ShipDirection.Right ? i : 0);
                var shipBlockPosY = ship.ShipPos.Y + (ship.Direction == ShipDirection.Right ? 0 : i);

                if (shipBlockPosX == pos.X && shipBlockPosY == pos.Y) {
                    return true;
                }
            }

            return false;
        }

        public static AttackResult AttackAtPos(Ship ship, Pos pos) {
            if (ship == null) {
                return AttackResult.Miss;
            }
            
            // Depending on the direction of the ship, set the local status tracker to hit
            if (ship.Direction == ShipDirection.Right) {
                ship.ShipStatuses[pos.X - ship.ShipPos.X] = ShipStatus.Hit;
            } else {
                ship.ShipStatuses[pos.Y - ship.ShipPos.Y] = ShipStatus.Hit;
            }

            return IsDestroyed(ship) ? AttackResult.Sink : AttackResult.Hit;
        }

        public static bool IsDestroyed(Ship ship) {
            foreach (var status in ship.ShipStatuses) {
                if (status == ShipStatus.Ok) {
                    return false;
                }
            }
            
            return true;
        }
        
        public static List<Ship> GenGameShipList() {
            var ships = new List<Ship>();

            // Generate list of ships based on their specs
            GenShips(ships, ShipType.Carrier, RuleType.CountCarrier, RuleType.SizeCarrier);
            GenShips(ships, ShipType.Battleship, RuleType.CountBattleship, RuleType.SizeBattleship);
            GenShips(ships, ShipType.Submarine, RuleType.CountSubmarine, RuleType.SizeSubmarine);
            GenShips(ships, ShipType.Cruiser, RuleType.CountCruiser, RuleType.SizeCruiser);
            GenShips(ships, ShipType.Patrol, RuleType.CountPatrol, RuleType.SizePatrol);
            
            return ships;
        }


        public static void GenShips(List<Ship> ships, ShipType type, RuleType countRule, RuleType sizeRule) {
            // Get ship size according to current rule set
            var size = ActiveGame.GetRuleVal(sizeRule);

            // Create clones of ships
            for (var i = 0; i < ActiveGame.GetRuleVal(countRule); i++) {
                var ship = new Ship(
                    ShipInitializers.DefaultShipSet.FirstOrDefault(m => m.Type.Equals(type))) {
                    Size = size
                };
                
                ships.Add(ship);
            }
        }
    }
}