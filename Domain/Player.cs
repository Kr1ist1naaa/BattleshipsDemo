using System.Collections.Generic;

namespace Domain {
    public class Player {
        public string Name;
        public HashSet<Pos> MovesAgainstThisPlayer  = new HashSet<Pos>();
        public List<Ship> Ships;

        public Player() { }

        public Player(string playerName, List<Ship> ships) {
            Name = playerName;
            Ships = ships;
        }
    }
}