namespace Domain {
    public class Move {
        private uint LocationX { get; }
        private uint LocationY { get; }

        public Move(uint moveLocationX, uint moveLocationY) {
            LocationX = moveLocationX;
            LocationY = moveLocationY;
        }
    }
}