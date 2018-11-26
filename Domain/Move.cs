namespace Domain {
    public class Move {
        private readonly Pos _pos;
        private readonly Player _fromPlayer;
        private readonly Player _toPlayer;
        private readonly AttackResult _attackResult;

        public Move(Player attackingPlayer, Player attackedPlayer, Pos pos, AttackResult attackResult) {
            _pos = new Pos(pos);
            _fromPlayer = attackingPlayer;
            _toPlayer = attackedPlayer;
            _attackResult = attackResult;
        }
    }
}