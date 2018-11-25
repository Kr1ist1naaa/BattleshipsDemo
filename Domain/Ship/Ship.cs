using System.Collections.Generic;

namespace Domain.Ship {
    public class Ship {
        public string Title { get; }
        public char Symbol { get; }
        public int Size { get; }
        public Pos ShipPos;
        public ShipDirection Direction;
        private ShipStatus[] _shipStatuses;

        public void SetLocation(Pos pos, ShipDirection direction) {
            Direction = direction;
            ShipPos = new Pos(pos);
        }

        public static bool CheckIfIntersect(Ship ship, Pos newPos, int newSize, ShipDirection newDirection) {
            // Ship hasn't been placed yet
            if (ship.ShipPos == null) {
                return false;
            }

            // This is one retarded way of doing it, but hey, it works...
            for (int i = 0; i < ship.Size; i++) {
                var shipPosX = ship.ShipPos.X + (ship.Direction == ShipDirection.Right ? i : 0);
                var shipPosY = ship.ShipPos.Y + (ship.Direction == ShipDirection.Right ? 0 : i);

                for (int j = 0; j < newSize; j++) {
                    var newPosX = newPos.X + (newDirection == ShipDirection.Right ? j : 0);
                    var newPosY = newPos.Y + (newDirection == ShipDirection.Right ? 0 : j);

                    if (shipPosX == newPosX && shipPosY == newPosY) {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsShipAtPos(Ship ship, Pos pos) {
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

        public static bool CanAttackShipAtPos(Ship ship, Pos pos) {
            // If there is no ship block at those coords
            if (!IsShipAtPos(ship, pos)) {
                return false;
            }

            // Depending on the direction of the ship, check if the ship block at those local coords is okay
            if (ship.Direction == ShipDirection.Right) {
                return ship._shipStatuses[pos.X - ship.ShipPos.X] == ShipStatus.Ok;
            } else {
                return ship._shipStatuses[pos.Y - ship.ShipPos.Y] == ShipStatus.Ok;
            }
        }

        public bool AttackShipAtPos(Pos pos) {
            // If there is no ship block at those coords or it has already been attacked
            if (!CanAttackShipAtPos(this, pos)) {
                return false;
            }

            // Depending on the direction of the ship, set the local status tracker to hit
            if (Direction == ShipDirection.Right) {
                _shipStatuses[pos.X - ShipPos.X] = ShipStatus.Hit;
            } else {
                _shipStatuses[pos.Y - ShipPos.Y] = ShipStatus.Hit;
            }

            return true;
        }

        public static bool IsDestroyed(Ship ship) {
            foreach (var status in ship._shipStatuses) {
                if (status == ShipStatus.Ok) {
                    return false;
                }
            }

            return true;
        }

        private void CreateShipStatusBlocks() {
            _shipStatuses = new ShipStatus[Size];

            for (int i = 0; i < Size; i++) {
                _shipStatuses[i] = ShipStatus.Ok;
            }
        }

        static Ship() {
            Carrier = new Ship("Carrier", 'C', 5);
            Battleship = new Ship("Battleship", 'B', 4);
            Submarine = new Ship("Submarine", 'S', 3);
            Cruiser = new Ship("Cruiser", 'R', 2);
            Patrol = new Ship("Patrol", 'P', 1);
        }

        private Ship(Ship ship) {
            Symbol = ship.Symbol;
            Title = ship.Title;
            Size = ship.Size;

            CreateShipStatusBlocks();
        }

        public Ship(string shipTitle, char shipSymbol, int shipSize) {
            Symbol = shipSymbol;
            Title = shipTitle;
            Size = shipSize;

            CreateShipStatusBlocks();
        }

        private static readonly Ship Carrier, Battleship, Submarine, Cruiser, Patrol;

        public static List<Ship> GenDefaultShipSet() {
            return new List<Ship> {
                new Ship(Carrier),
                new Ship(Battleship),
                new Ship(Submarine),
                new Ship(Cruiser),
                new Ship(Patrol)
            };
        }
    }
}