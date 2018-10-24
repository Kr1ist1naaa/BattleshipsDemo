using Domain.Board;

namespace Domain {
    public class Player {
        private string name { get; }
        private ShipBoard ShipBoard { get; }
        private MoveBoard MoveBoard { get; }
        
        public Player(uint boardSizeX, uint boardSizeY) {
            ShipBoard = new ShipBoard(boardSizeX, boardSizeY);
            MoveBoard = new MoveBoard(boardSizeX, boardSizeY);
        }
    }
}