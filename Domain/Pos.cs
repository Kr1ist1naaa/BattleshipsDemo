namespace Domain {
    public class Pos {
        public int X;
        public int Y;

        public Pos() { }

        public Pos(int x, int y) {
            X = x;
            Y = y;
        }

        public Pos(Pos pos) {
            X = pos.X;
            Y = pos.Y;
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }

            if (typeof(Pos) != obj.GetType()) {
                return false;
            }

            var other = (Pos) obj;

            if (X != other.X) {
                return false;
            }

            if (Y != other.Y) {
                return false;
            }

            return true;
        }

        public override int GetHashCode() {
            var hash = 3;

            hash = 53 * hash + X;
            hash = 53 * hash + Y;

            return hash;
        }
    }
}