using System;
using System.Collections.Generic;
using MenuSystem;

namespace Domain {
    public class GameManager {
        private readonly Menu _menu;
        private readonly List<Game> _games;
        private int _currentGame = -1;
        
        public GameManager() {
            _menu = new Menu();
            _games = new List<Game>();
        }

        public void Run() {
            NewGame(2, 10, 10);

            _games[_currentGame].Run();
        }

        private void NewGame(int playerCount, int boardSizeX, int boardSizeY) {
            CheckParams(playerCount, boardSizeX, boardSizeY);
            
            var game = new Game(_menu, ++_currentGame, playerCount, boardSizeX, boardSizeY);
            _games.Add(game);
        }
        
        private static void CheckParams(int playerCount, int boardSizeX, int boardSizeY) {
            if (boardSizeX < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeX));
            }
            
            if (boardSizeY < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeY));
            }

            if (playerCount < 2) {
                throw new ArgumentOutOfRangeException(nameof(playerCount));
            }
        }
        
    }
}