namespace DAL {
    public class Move {
        public int Id { get; set; }
        public Game Game { get; set; }
        public Player FromPlayer { get; set; }
        public Player ToPlayer { get; set; }
        
        public int MoveResult { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}