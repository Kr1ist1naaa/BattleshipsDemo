using System.Collections.Generic;
using System.Linq;
using Domain.DomainRule;

namespace Domain.Ship {
    public static class Ships {
        private static readonly HashSet<DomainShip.Ship> BaseShipSet = new HashSet<DomainShip.Ship> {
            new DomainShip.Ship {
                Type = ShipType.Carrier,
                SizeRule = RuleType.SizeCarrier,
                Title = "Carrier",
                Symbol = 'C'
            },
            new DomainShip.Ship {
                Type = ShipType.Battleship,
                SizeRule = RuleType.SizeBattleship,
                Title = "Battleship",
                Symbol = 'B'
            },
            new DomainShip.Ship {
                Type = ShipType.Submarine,
                SizeRule = RuleType.SizeSubmarine,
                Title = "Submarine",
                Symbol = 'S'
            },
            new DomainShip.Ship {
                Type = ShipType.Cruiser,
                SizeRule = RuleType.SizeCruiser,
                Title = "Cruiser",
                Symbol = 'c'
            },
            new DomainShip.Ship {
                Type = ShipType.Patrol,
                SizeRule = RuleType.SizePatrol,
                Title = "Patrol",
                Symbol = 'p'
            }
        };

        public static List<DomainShip.Ship> GenShipSet() {
            var ships = new List<DomainShip.Ship>();
            
            // Update ship set's ship sizes based on defined rules
            foreach (var ship in BaseShipSet) {
                ship.Size = Rules.GetVal(ship.SizeRule);
            }

            // Generate list of ships based on their specific counts
            for (var i = 0; i < Rules.GetVal(RuleType.CountCarrier); i++)
                ships.Add(new DomainShip.Ship(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Carrier))));
            for (var i = 0; i < Rules.GetVal(RuleType.CountBattleship); i++)
                ships.Add(new DomainShip.Ship(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Battleship))));
            for (var i = 0; i < Rules.GetVal(RuleType.CountSubmarine); i++)
                ships.Add(new DomainShip.Ship(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Submarine))));
            for (var i = 0; i < Rules.GetVal(RuleType.CountCruiser); i++)
                ships.Add(new DomainShip.Ship(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Cruiser))));
            for (var i = 0; i < Rules.GetVal(RuleType.CountPatrol); i++)
                ships.Add(new DomainShip.Ship(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Patrol))));

            return ships;
        }
    }
}