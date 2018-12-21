using System.Collections.Generic;

namespace DAL {
    public class Player {
        public int Id { get; set; }
        public Game Game { get; set; }

        public string Name { get; set; }
        public HashSet<MovesAgainstPlayer> MovesAgainstThisPlayer { get; set; }
        public List<Ship> Ships { get; set; }
    }
}