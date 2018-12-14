namespace Domain {
    public class Move {
        public Pos Pos;
        public Player FromPlayer;
        public Player ToPlayer;
        public AttackResult AttackResult;

        public Move(Player attackingPlayer, Player attackedPlayer, Pos pos, AttackResult attackResult) {
            Pos = new Pos(pos);
            FromPlayer = attackingPlayer;
            ToPlayer = attackedPlayer;
            AttackResult = attackResult;
        }
        
        public Move () {}
    }
}