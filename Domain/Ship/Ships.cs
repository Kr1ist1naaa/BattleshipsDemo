using System.Collections.Generic;
using System.Linq;
using Domain.Rule;

namespace Domain.Ship {
    public static class Ships {
        private static readonly HashSet<BaseShip> BaseShipSet = new HashSet<BaseShip> {
            new BaseShip {
                Type = ShipType.Carrier,
                SizeRule = RuleType.SizeCarrier,
                Title = "Carrier",
                Symbol = 'C'
            },
            new BaseShip {
                Type = ShipType.Battleship,
                SizeRule = RuleType.SizeBattleship,
                Title = "Battleship",
                Symbol = 'B'
            },
            new BaseShip {
                Type = ShipType.Submarine,
                SizeRule = RuleType.SizeSubmarine,
                Title = "Submarine",
                Symbol = 'S'
            },
            new BaseShip {
                Type = ShipType.Cruiser,
                SizeRule = RuleType.SizeCruiser,
                Title = "Cruiser",
                Symbol = 'c'
            },
            new BaseShip {
                Type = ShipType.Patrol,
                SizeRule = RuleType.SizePatrol,
                Title = "Patrol",
                Symbol = 'p'
            }
        };

        public static List<BaseShip> GenShipSet() {
            var ships = new List<BaseShip>();
            
            // Update ship set's ship sizes based on defined rules
            foreach (var ship in BaseShipSet) {
                ship.Size = Rules.GetVal(ship.SizeRule);
            }

            // Generate list of ships based on their specific counts
            for (var i = 0; i < Rules.GetVal(RuleType.CountCarrier); i++)
                ships.Add(new BaseShip(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Carrier))));
            for (var i = 0; i < Rules.GetVal(RuleType.CountBattleship); i++)
                ships.Add(new BaseShip(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Battleship))));
            for (var i = 0; i < Rules.GetVal(RuleType.CountSubmarine); i++)
                ships.Add(new BaseShip(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Submarine))));
            for (var i = 0; i < Rules.GetVal(RuleType.CountCruiser); i++)
                ships.Add(new BaseShip(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Cruiser))));
            for (var i = 0; i < Rules.GetVal(RuleType.CountPatrol); i++)
                ships.Add(new BaseShip(BaseShipSet.FirstOrDefault(m => m.Type.Equals(ShipType.Patrol))));

            return ships;
        }
    }
}