namespace Domain.Board {
    public class MoveBoard {
        private uint SizeX {get;}
        private uint SizeY {get;}
        private MoveBoardState[][] Board { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardSizeX"></param>
        /// <param name="boardSizeY"></param>
        public MoveBoard(uint boardSizeX, uint boardSizeY) {
            SizeX = boardSizeX;
            SizeY = boardSizeY;
            
            Board = new MoveBoardState[SizeX][];
            for (var i = 0; i < SizeX; i++) {
                Board[i] = new MoveBoardState[SizeY];
            }
        }
    }
}