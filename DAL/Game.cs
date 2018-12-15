using System.Collections.Generic;

namespace DAL {
    public class Game {
        public int Id { get; set; }
        
        public string Date { get; set; }
        
        public List<Move> Moves { get; set; }
        public HashSet<Rule> Rules { get; set; }
        public List<Player> Players { get; set; }
        
        public string Winner { get; set; }
        public int TurnCount { get; set; }
    }
}