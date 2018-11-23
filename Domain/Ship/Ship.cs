using System.Collections.Generic;

namespace Domain.Ship {
    public class Ship {
        private static readonly Ship Carrier, Battleship, Submarine, Cruiser, Patrol;

        static Ship() {
            Carrier = new Ship("Carrier", 'C', 5);
            Battleship = new Ship("Battleship", 'B', 4);
            Submarine = new Ship("Submarine", 'S', 3);
            Cruiser = new Ship("Cruiser", 'R', 2);
            Patrol = new Ship("Patrol", 'P', 1);
        }

        public string Title { get; }
        public char Symbol { get; }
        public int Size { get; }

        private Ship(Ship ship) {
            Symbol = ship.Symbol;
            Title = ship.Title;
            Size = ship.Size;
        }

        public Ship(string shipTitle, char shipSymbol, int shipSize) {
            Symbol = shipSymbol;
            Title = shipTitle;
            Size = shipSize;
        }

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