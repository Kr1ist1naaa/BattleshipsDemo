using System;
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

        public bool CheckIfIntersect(Pos newPos, int newSize, ShipDirection newDirection, bool ruleBoatsCanTouch) {
            // Ship hasn't been placed yet
            if (ShipPos == null) {
                return false;
            }

            // This is one retarded way of doing it, but hey, it works...
            for (int i = 0; i < Size; i++) {
                var shipPosX = ShipPos.X + (Direction == ShipDirection.Right ? i : 0);
                var shipPosY = ShipPos.Y + (Direction == ShipDirection.Right ? 0 : i);

                for (int j = 0; j < newSize; j++) {
                    var newPosX = newPos.X + (newDirection == ShipDirection.Right ? j : 0);
                    var newPosY = newPos.Y + (newDirection == ShipDirection.Right ? 0 : j);

                    if (ruleBoatsCanTouch) {
                        if (shipPosX == newPosX && shipPosY == newPosY) {
                            return true;
                        }
                    } else {
                        if (newPosX > shipPosX - 2 && newPosX < shipPosX + 2 && 
                            newPosY > shipPosY - 2 && newPosY < shipPosY + 2) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool IsAtPos(Pos pos) {
            // Ship hasn't been placed yet
            if (ShipPos == null) {
                return false;
            }

            for (int i = 0; i < Size; i++) {
                var shipBlockPosX = ShipPos.X + (Direction == ShipDirection.Right ? i : 0);
                var shipBlockPosY = ShipPos.Y + (Direction == ShipDirection.Right ? 0 : i);

                if (shipBlockPosX == pos.X && shipBlockPosY == pos.Y) {
                    return true;
                }
            }

            return false;
        }

        public AttackResult AttackAtPos(Pos pos) {
            if (!IsAtPos(pos)) {
                // Should be checked beforehand
                throw new ArgumentException(nameof(pos));
            }
            
            // Depending on the direction of the ship, set the local status tracker to hit
            if (Direction == ShipDirection.Right) {
                _shipStatuses[pos.X - ShipPos.X] = ShipStatus.Hit;
            } else {
                _shipStatuses[pos.Y - ShipPos.Y] = ShipStatus.Hit;
            }

            return IsDestroyed() ? AttackResult.Sink : AttackResult.Hit;
        }

        public bool IsDestroyed() {
            foreach (var status in _shipStatuses) {
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
            Cruiser = new Ship("Cruiser", 'c', 2);
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