namespace Domain {
    public class Move {
        private readonly Pos _pos;
        private readonly Player _fromPlayer;
        private readonly Player _toPlayer;

        public Move(Player attackingPlayer, Player attackedPlayer, Pos pos) {
            _pos = new Pos(pos);
            _fromPlayer = attackingPlayer;
            _toPlayer = attackedPlayer;
        }
    }
}