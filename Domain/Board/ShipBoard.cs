namespace Domain.Board {
    public class ShipBoard {
        private uint SizeX {get;}
        private uint SizeY {get;}
        private Ship.Ship[][] Slots { get; }
        
        public ShipBoard(uint boardSizeX, uint boardSizeY) {
            SizeX = boardSizeX;
            SizeY = boardSizeY;
            
            Slots = new Ship.Ship[SizeX][];
            for (var i = 0; i < SizeX; i++) {
                Slots[i] = new Ship.Ship[SizeY];
            }
        }
    }
}