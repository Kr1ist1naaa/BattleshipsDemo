namespace DAL {
    public class Move {
        public int MoveId { get; set; }
        
        public string FromPlayer { get; set; }
        public string ToPlayer { get; set; }
        public int AttackResult { get; set; }
        
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}