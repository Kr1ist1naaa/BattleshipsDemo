using System.Collections.Generic;

namespace Domain {
    public class Player {
        private Board.Board Board { get; }
        private List<Move> Moves { get; }
        
        public Player(uint boardSizeX, uint boardSizeY) {
            Board = new Board.Board(boardSizeX, boardSizeY);
            Moves = new List<Move>();
        }
    }
}