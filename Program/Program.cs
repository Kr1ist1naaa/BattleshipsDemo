using System;
using System.Collections.Generic;
using Domain;

namespace Program {
    public static class Program {
        private static readonly Menu _menu = new Menu();
        private static readonly List<Game> _games = new List<Game>();
        private static int _gameNumber;
        
        public static void Main(string[] args) {
            Console.WriteLine("Welcome to Connect-X! The classical Connect-X family game.\n");

            while (true) {
                var game = new Game(_menu, _gameNumber);
                _games.Add(game);
                
                Console.WriteLine($"Initializing game nr {_gameNumber}:");
                
                game.InitializeRules();
                game.InitializePlayers();
                game.InitializeShips();
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