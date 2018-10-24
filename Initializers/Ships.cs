using Domain.Ship;

namespace Initializers {
    public static class Ships {
        public static readonly Ship Carrier, Battleship, Submarine, Cruiser, Patrol;
        
        static Ships() {
            Carrier = new Ship(1, 5);
            Battleship = new Ship(1, 4);
            Submarine = new Ship(1, 3);
            Cruiser = new Ship(1, 2);
            Patrol = new Ship(1, 1);
        }
    }
}