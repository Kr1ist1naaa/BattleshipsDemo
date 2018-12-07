using System;
using System.Collections.Generic;
using System.Threading;
using Domain;

namespace Program {
    public static class Program {
        private static readonly Menu _menu = new Menu();
        private static readonly List<Game> _games = new List<Game>();
        private static int _gameNumber;
        
        public static void Main(string[] args) {
            Console.WriteLine("Welcome to Connect-X! The classical Connect-X family game.");
            
            /*Console.WriteLine("Press ESC to stop");
            do {
                while (!Console.KeyAvailable) {
                    Thread.Sleep(500);
                    Console.WriteLine(123);
                }       
            } while (Console.ReadKey(true).Key != ConsoleKey.LeftArrow);*/
            

            while (true) {
                var game = new Game(_menu, _gameNumber);
                _games.Add(game);
                
                Console.WriteLine($"  - starting game nr {_gameNumber}...");
                Console.ReadKey(true);
                
                game.InitializeRules();
                game.InitializePlayers();
                game.InitializeShips();
                game.StartGame();

                var newGame = _menu.AskNewGame();
                if (!newGame) break;

                _gameNumber++;
            }
        }
    }
}