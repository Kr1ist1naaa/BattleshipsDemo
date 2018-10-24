namespace Domain.Board {
    public class Board {
        private uint SizeX {get;}
        private uint SizeY {get;}
        private BoardSlot[][] BoardSlots { get; }
        
        public Board(uint boardSizeX, uint boardSizeY) {
            SizeX = boardSizeX;
            SizeY = boardSizeY;
            
            BoardSlots = new BoardSlot[SizeX][];
            for (var i = 0; i < SizeX; i++) {
                BoardSlots[i] = new BoardSlot[SizeY];
            }
        }
    }
}