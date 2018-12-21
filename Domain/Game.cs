using System.Collections.Generic;
using Domain.DomainRule;

namespace Domain {
    public class Game {
        public Player Winner;
        public List<Move> Moves;
        public List<Player> Players;
        public HashSet<Rule> Rules;
        public int TurnCount;
        public int? GameId = null;

        public Game(List<Player> players) {
            Winner = null;
            Moves = new List<Move>();
            Players = players;
        }

        public Game() { }
    }
}