namespace Domain.Board {
    public class MoveBoard {
        private uint SizeX {get;}
        private uint SizeY {get;}
        private MoveBoardState[][] Slots { get; }
        
        public MoveBoard(uint boardSizeX, uint boardSizeY) {
            SizeX = boardSizeX;
            SizeY = boardSizeY;
            
            Slots = new MoveBoardState[SizeX][];
            for (var i = 0; i < SizeX; i++) {
                Slots[i] = new MoveBoardState[SizeY];
            }
        }
    }
}