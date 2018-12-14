using System.Collections.Generic;

namespace DAL {
    public class Player {
        public int PlayerId { get; set; }

        public string Name { get; set; }
        public HashSet<Pos> MovesAgainstThisPlayer { get; set; }
        public List<Ship> Ships { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}