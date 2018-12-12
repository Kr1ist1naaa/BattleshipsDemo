using System;
using System.Collections.Generic;
using Domain.Rule;

namespace Domain.Ship {
    public class Ship {
        public readonly string Title;
        public readonly char Symbol;
        public readonly int Size;
        private Pos _shipPos;
        private ShipDirection _direction;
        private ShipStatus[] _shipStatuses;

        public Ship(string shipTitle, char shipSymbol, int shipSize) {
            Symbol = shipSymbol;
            Title = shipTitle;
            Size = shipSize;

            CreateShipStatusBlocks();
        }
        
        private Ship(Ship ship, int size) {
            Symbol = ship.Symbol;
            Title = ship.Title;
            Size = size;

            CreateShipStatusBlocks();
        }
        
        public void SetLocation(Pos pos, ShipDirection direction) {
            _direction = direction;
            _shipPos = new Pos(pos);
        }

        public bool CheckIfIntersect(Pos newPos, int newSize, ShipDirection newDirection, int padding) {
            // Ship hasn't been placed yet
            if (_shipPos == null) {
                return false;
            }

            // This is one retarded way of doing it, but hey, it works...
            for (int offset = 0; offset < newSize; offset++) {
                var newPosOffsetX = newPos.X + (newDirection == ShipDirection.Right ? offset : 0);
                var newPosOffsetY = newPos.Y + (newDirection == ShipDirection.Right ? 0 : offset);

                for (int i = 0; i < Size; i++) {
                    var shipPosX = _shipPos.X + (_direction == ShipDirection.Right ? i : 0);
                    var shipPosY = _shipPos.Y + (_direction == ShipDirection.Right ? 0 : i);
                    
                    if (shipPosX >= newPosOffsetX - padding && shipPosX <= newPosOffsetX + padding && 
                        shipPosY >= newPosOffsetY - padding && shipPosY <= newPosOffsetY + padding) {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsAtPos(Pos pos) {
            // Ship hasn't been placed yet
            if (_shipPos == null) {
                return false;
            }

            for (int i = 0; i < Size; i++) {
                var shipBlockPosX = _shipPos.X + (_direction == ShipDirection.Right ? i : 0);
                var shipBlockPosY = _shipPos.Y + (_direction == ShipDirection.Right ? 0 : i);

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
            if (_direction == ShipDirection.Right) {
                _shipStatuses[pos.X - _shipPos.X] = ShipStatus.Hit;
            } else {
                _shipStatuses[pos.Y - _shipPos.Y] = ShipStatus.Hit;
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

        
        
        
        
        private static readonly Ship Carrier, Battleship, Submarine, Cruiser, Patrol;
        
        static Ship() {
            Carrier = new Ship("Carrier", 'C', 5);
            Battleship = new Ship("Battleship", 'B', 4);
            Submarine = new Ship("Submarine", 'S', 3);
            Cruiser = new Ship("Cruiser", 'c', 2);
            Patrol = new Ship("Patrol", 'p', 1);
        }

        public static List<Ship> GenShipSet() {
            var ships = new List<Ship>();
            
            // Create the ships defined by rules
            for (var i = 0; i < Rules.GetVal(RuleType.CountCarrier); i++) {
                ships.Add(new Ship(Carrier, Rules.GetVal(RuleType.SizeCarrier)));
            }
            for (var i = 0; i < Rules.GetVal(RuleType.CountBattleship); i++) {
                ships.Add(new Ship(Battleship, Rules.GetVal(RuleType.SizeBattleship)));
            }
            for (var i = 0; i < Rules.GetVal(RuleType.CountSubmarine); i++) {
                ships.Add(new Ship(Submarine, Rules.GetVal(RuleType.SizeSubmarine)));
            }
            for (var i = 0; i < Rules.GetVal(RuleType.CountCruiser); i++) {
                ships.Add(new Ship(Cruiser, Rules.GetVal(RuleType.SizeCruiser)));
            }
            for (var i = 0; i < Rules.GetVal(RuleType.CountPatrol); i++) {
                ships.Add(new Ship(Patrol, Rules.GetVal(RuleType.SizePatrol)));
            }

            return ships;
        }
    }
}