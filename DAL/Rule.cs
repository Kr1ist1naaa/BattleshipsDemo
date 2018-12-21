namespace DAL {
    public class Rule {
        public int Id { get; set; }
        
        public int GameId { get; set; }
        public Game Game { get; set; }
        
        public int RuleType { get; set; }
        public int Value { get; set; }
    }
}