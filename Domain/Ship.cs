namespace Domain {
    public class Ship {
        public ShipType Type;
        public RuleType SizeRule;
        public string Title;
        public char Symbol;
        public int Size;

        public Pos ShipPos;
        public ShipDirection Direction;
        public ShipStatus[] ShipStatuses;
        public bool IsPlaced;

        public Ship(Ship ship) {
            SizeRule = ship.SizeRule;
            Symbol = ship.Symbol;
            Title = ship.Title;
            Size = ship.Size;
            Type = ship.Type;
        }

        public Ship() { }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            if (typeof(Ship) != obj.GetType()) {
                return false;
            }

            var other = (Ship) obj;

            if (Type != other.Type) {
                return false;
            }

            if (Symbol != other.Symbol) {
                return false;
            }

            return true;
        }

        private static ShipStatus[] GenStatusBlocks(int size) {
            var statuses = new ShipStatus[size];
            
            for (int i = 0; i < size; i++) {
                statuses[i] = ShipStatus.Ok;
            }

            return statuses;
        }

        public override int GetHashCode() {
            var hash = 3;

            hash = 53 * hash + Symbol.GetHashCode();

            return hash;
        }

        public void SetLocation(Pos pos, ShipDirection direction) {
            ShipStatuses = GenStatusBlocks(Size);
            ShipPos = new Pos(pos);
            Direction = direction;
            IsPlaced = true;
        }
    }
}