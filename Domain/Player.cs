using System.Collections.Generic;
using Domain.Rule;
using Domain.Ship;

namespace Domain {
    public class Player {
        public string Name;
        public HashSet<Pos> MovesAgainstThisPlayer;
        public List<BaseShip> Ships;

        public Player() { }

        public Player(string playerName) {
            MovesAgainstThisPlayer = new HashSet<Pos>();
            Name = playerName;

            // Generate a set of ships for the player based on the current rules
            Ships = Ship.Ships.GenShipSet();
        }

        private static bool CheckIfPosInBoard(Pos pos) {
            var boardSize = Rules.GetVal(RuleType.BoardSize);

            // Out of bounds
            if (pos.X < 0 || pos.X >= boardSize) return false;
            if (pos.Y < 0 || pos.Y >= boardSize) return false;

            return true;
        }

        public AttackResult AttackAtPos(Pos pos) {
            if (!CheckIfPosInBoard(pos)) {
                return AttackResult.InvalidAttack;
            }

            if (MovesAgainstThisPlayer.Contains(pos)) {
                return AttackResult.DuplicateAttack;
            }

            MovesAgainstThisPlayer.Add(pos);

            var ship = GetShipAtPosOrNull(pos);
            if (ship == null) {
                return AttackResult.Miss;
            }

            return ship.AttackAtPos(pos);
        }

        public BaseShip GetShipAtPosOrNull(Pos pos) {
            foreach (var ship in Ships) {
                if (ship.IsAtPos(pos)) {
                    return ship;
                }
            }

            return null;
        }

        public bool CheckIfValidPlacementPos(Pos pos, int shipSize, ShipDirection direction) {
            // Check if position is off board
            if (!CheckIfPosInBoard(pos)) {
                return false;
            }

            // Find ship's furthest point
            var maxPos = new Pos(
                pos.X + (direction == ShipDirection.Right ? shipSize : 0),
                pos.Y + (direction == ShipDirection.Right ? 0 : shipSize)
            );

            // Check if max position is off board
            if (!CheckIfPosInBoard(maxPos)) {
                return false;
            }

            var padding = Rules.GetVal(RuleType.ShipPadding);

            // Check if any ships already exist at that location
            foreach (var ship in Ships) {
                if (ship.CheckIfIntersect(pos, shipSize, direction, padding)) {
                    return false;
                }
            }

            return true;
        }

        public bool IsAlive() {
            // Count each ship block individually, checking if there is at least one that's not hit
            foreach (var ship in Ships) {
                if (!ship.IsDestroyed()) {
                    return true;
                }
            }

            return false;
        }
    }
}