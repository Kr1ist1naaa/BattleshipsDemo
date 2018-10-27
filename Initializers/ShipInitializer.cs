using System.Collections.Generic;
using Domain.Ship;

namespace Initializers {
    public static class ShipInitializer {
        public static readonly Ship Carrier, Battleship, Submarine, Cruiser, Patrol;

        /// <summary>
        /// 
        /// </summary>
        static ShipInitializer() {
            Carrier    = new Ship("Carrier",    'C', 5);
            Battleship = new Ship("Battleship", 'B', 4);
            Submarine  = new Ship("Submarine",  'S', 3);
            Cruiser    = new Ship("Cruiser",    'R', 2);
            Patrol     = new Ship("Patrol",     'P', 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Ship> GenDefaultShipSet() {
            return new List<Ship> {
                new Ship("Carrier",    'C', 5),
                new Ship("Battleship", 'B', 4),
                new Ship("Submarine",  'S', 3),
                new Ship("Cruiser",    'R', 2),
                new Ship("Patrol",     'P', 1)
            };
        }
    }
}