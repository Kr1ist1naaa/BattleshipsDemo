using System.Collections.Generic;
using Domain;

namespace Program {
    public static class Program {
        private static readonly Menu _menu = new Menu();
        private static readonly List<Game> _games = new List<Game>();
        private static int _gameNumber;
        
        public static void Main(string[] args) {
            while (true) {
                var game = new Game(_menu, _gameNumber);
                _games.Add(game);
                game.StartGame();
                
                var newGame = _menu.AskNewGame();
                if (!newGame) {
                    break;
                }

                _gameNumber++;
            }
        }
    }
}