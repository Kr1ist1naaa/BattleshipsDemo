namespace Domain.Ship {
    /// <summary>
    /// 
    /// </summary>
    public class Ship {
        private string Title { get; }
        private char Symbol { get; }
        private ShipStatus[] StatusBlocks { get; }
        public uint Size { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipTitle"></param>
        /// <param name="shipSymbol"></param>
        /// <param name="shipSize"></param>
        public Ship(string shipTitle, char shipSymbol, uint shipSize) {
            StatusBlocks = new ShipStatus[shipSize];
            Symbol = shipSymbol;
            Title = shipTitle;
            Size = shipSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool Hit(uint offset) {
            if (StatusBlocks.Length <= offset) return false;
            
            StatusBlocks[offset] = ShipStatus.Hit;
            return true;
        }
    }
}