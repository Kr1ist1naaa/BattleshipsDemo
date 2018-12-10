using System;
using System.Collections.Generic;
using System.Threading;
using Domain;

namespace Program {
    public static class Program {
        private static readonly Menu _menu = new Menu();
        private static readonly List<Game> _games = new List<Game>();
        
        public static void Main(string[] args) {
            Console.WriteLine("Welcome to Connect-X! The classical Connect-X family game.");

            while (true) {
                var game = new Game(_menu);
                _games.Add(game);

                game.InitializeRules();
                game.InitializePlayers();
                game.InitializeShips();
                game.StartGame();

                var newGame = _menu.AskNewGame();
                if (!newGame) break;
            }
        }
    }
}