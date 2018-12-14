using System.Collections.Generic;

namespace Domain {
    public class BaseGame {
        public Player Winner;
        public List<Move> Moves;
        public List<Player> Players;
        public int TurnCount;

        public BaseGame(List<Player> players) {
            Winner = null;
            Moves = new List<Move>();
            Players = players;
        }

        public BaseGame() {
            
        }
    }
}