namespace Domain {
    public class Move {
        public readonly Pos Pos;
        private readonly Player _fromPlayer;
        private readonly Player _toPlayer;
        public readonly AttackResult AttackResult;

        public Move(Player attackingPlayer, Player attackedPlayer, Pos pos, AttackResult attackResult) {
            Pos = new Pos(pos);
            _fromPlayer = attackingPlayer;
            _toPlayer = attackedPlayer;
            AttackResult = attackResult;
        }
    }
}