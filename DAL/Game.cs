using System.Collections.Generic;

namespace DAL {
    public class Game {
        public int GameId { get; set; }
        
        public string SaveName { get; set; }
        public string Date { get; set; }
        
        public List<Move> Moves { get; set; }
        public List<Rule> Rules { get; set; }
        public List<Player> Players { get; set; }
        
        public string Winner { get; set; }
        public int TurnCount { get; set; }
    }
}