using System;
using Domain.Rule;

namespace Domain.Ship {
    public class BaseShip {
        public ShipType Type;
        public RuleType SizeRule;
        public string Title;
        public char Symbol;
        public int Size;
        
        private Pos _shipPos;
        private ShipDirection _direction;
        private ShipStatus[] _shipStatuses;

        public BaseShip(BaseShip baseShip) {
            SizeRule = baseShip.SizeRule;
            Symbol = baseShip.Symbol;
            Title = baseShip.Title;
            Size = baseShip.Size;
            Type = baseShip.Type;
            
            CreateShipStatusBlocks();
        }
        
        public BaseShip() {}
        
        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            if (typeof(BaseShip) != obj.GetType()) {
                return false;
            }

            var other = (BaseShip) obj;

            if (Type != other.Type) {
                return false;
            }
            
            if (Symbol != other.Symbol) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() {
            var hash = 3;

            if (Symbol != null) hash = 53 * hash + Symbol.GetHashCode();

            return hash;
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
    }
}