namespace Domain {
    public class Move {
        private int _posX;
        private int _posY;
        private Player _fromPlayer;
        private Player _toPlayer;    

        public Move(Player attackingPlayer, Player attackedPlayer, int moveLocationX, int moveLocationY) {
            _posX = moveLocationX;
            _posY = moveLocationY;
            _fromPlayer = attackingPlayer;
            _toPlayer = attackedPlayer;
        }
    }
}