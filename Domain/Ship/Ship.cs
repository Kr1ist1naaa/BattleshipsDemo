namespace Domain.Ship {
    public class Ship {
        private string Title { get; }
        private char Symbol { get; }
        public int Size { get; }

        public Ship(string shipTitle, char shipSymbol, int shipSize) {
            Symbol = shipSymbol;
            Title = shipTitle;
            Size = shipSize;
        }
    }
}