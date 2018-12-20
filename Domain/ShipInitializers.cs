using System.Collections.Generic;

namespace Domain {
    public static class ShipInitializers {
        public static readonly HashSet<Ship> DefaultShipSet = new HashSet<Ship> {
            new Ship {
                Type = ShipType.Carrier,
                SizeRule = RuleType.SizeCarrier,
                Title = "Carrier",
                Symbol = 'C'
            },
            new Ship {
                Type = ShipType.Battleship,
                SizeRule = RuleType.SizeBattleship,
                Title = "Battleship",
                Symbol = 'B'
            },
            new Ship {
                Type = ShipType.Submarine,
                SizeRule = RuleType.SizeSubmarine,
                Title = "Submarine",
                Symbol = 'S'
            },
            new Ship {
                Type = ShipType.Cruiser,
                SizeRule = RuleType.SizeCruiser,
                Title = "Cruiser",
                Symbol = 'c'
            },
            new Ship {
                Type = ShipType.Patrol,
                SizeRule = RuleType.SizePatrol,
                Title = "Patrol",
                Symbol = 'p'
            }
        };
    }
}