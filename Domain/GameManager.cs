using System.Collections.Generic;
using MenuSystem;

namespace Domain {
    public class GameManager {
        private readonly Menu _menu;
        private readonly List<Game> _games;
        private int _currentGame = 0;
        
        public GameManager() {
            _menu = new Menu();
            _games = new List<Game>();
        }

        public void Run() {
            var firstLoop = true;

            while (true) {
                firstLoop = _menu.AskGameDetails(firstLoop, out var playerCount, out var boardSizeX, out var boardSizeY);
                var boardSize = new Pos(boardSizeX, boardSizeY);
                
                // Check if user-provided params are valid
                var isValidParams = checkValidParams(playerCount, boardSize);
                if (!isValidParams) {
                    continue;
                }
                
                var game = new Game(_menu, ++_currentGame, playerCount, boardSize);
                _games.Add(game);
                
                game.StartGame();
                
                var newGame = _menu.AskNewGame();
                if (!newGame) {
                    break;
                }
            }
        }

        private static bool checkValidParams(int playerCount, Pos boardSize) {
            if (boardSize.X < 2 || boardSize.X > 256) {
                return false;
            }
            
            if (boardSize.Y < 2 || boardSize.Y > 256) {
                return false;
            }

            if (playerCount < 2 || playerCount > 64) {
                return false;
            }

            return true;
        }
    }
}