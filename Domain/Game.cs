using System.Collections.Generic;

namespace Domain {
    public class Game {
        private Player[] Players { get; } 
        private List<Rule> Rules { get; } 
        private List<Move> Moves { get; }

        public Game(uint playerCount, uint boardSizeX, uint boardSizeY) {
            Players = new Player[playerCount];
            Moves = new List<Move>();
            Rules = new List<Rule>();
            
            for (var i = 0; i < playerCount; i++) {
                Players[i] = new Player(boardSizeX, boardSizeY);
            }
        }
    }
}