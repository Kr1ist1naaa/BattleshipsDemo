namespace Domain.Ship {
    public class Ship {
        private uint SizeX { get; }
        private uint SizeY { get; }
        private ShipStatus[][] Status;

        public Ship(uint shipSizeX, uint shipSizeY) {
            SizeX = shipSizeX;
            SizeY = shipSizeY;
            
            Status = new ShipStatus[SizeX][];
            for (var i = 0; i < SizeX; i++) {
                Status[i] = new ShipStatus[SizeY];
                
                for (var j = 0; j < SizeY; j++) {
                    Status[i][j] = ShipStatus.Ok;
                }
            }
        }
    }
}