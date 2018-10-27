using System.Collections.Generic;

namespace Domain {
    public class Game {
        private List<Player> Players { get; } 
        private List<Rule> Rules { get; } 
        private List<Move> Moves { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerCount"></param>
        /// <param name="boardSizeX"></param>
        /// <param name="boardSizeY"></param>
        public Game(uint playerCount, uint boardSizeX, uint boardSizeY) {
            Players = new List<Player>();
            Moves = new List<Move>();
            Rules = new List<Rule>();

            for (var i = 0; i < playerCount; i++) {
                Players.Add(new Player(boardSizeX, boardSizeY, "Asd123"));
            }
        }
    }
}