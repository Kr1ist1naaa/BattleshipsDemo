using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoardUI;
using Domain;
using Domain.Rule;
using Domain.Ship;

namespace GameSystem {
    public static class Game {
        public static void LoadGame() {
            // Todo: this
        }

        public static void NewGame() {
            Console.Clear();
            Console.WriteLine("Welcome to Battleships - the classical battle ship game!\n");

            Console.WriteLine("The active rules are:");
            foreach (var rule in Rules.RuleSet) {
                Console.WriteLine($"  - {rule.RuleType}: {rule.Value}");
            }
            
            if (!AskYesNoQuestion("\nContinue and create players (y/n): ")) {
                Console.WriteLine("Stopping game...");
                Console.ReadKey(true);
                return;
            }

            var players = InitializePlayers();

            if (!AskYesNoQuestion("\nContinue and place ships (y/n): ")) {
                Console.WriteLine("Stopping game...");
                Console.ReadKey(true);
                return;
            }
            
            InitializeShips(players);
            
            Console.Clear();
            if (!AskYesNoQuestion("Continue and start game (y/n): ")) {
                Console.WriteLine("Stopping game...");
                Console.ReadKey(true);
                return;
            }
            
            GameLoop(players);
        }

        private static bool AskYesNoQuestion(string question) {
            bool? input = null;

            while (true) {
                Console.Write(question);
                
                switch (Console.ReadLine()?.ToLower().Trim()) {
                    case "":
                    case "y":
                    case "yes":
                        input = true;
                        break;
                    case "n":
                    case "no":
                        input = false;
                        break;
                }

                if (input == null) {
                    Console.WriteLine("Unknown command...");
                    Console.ReadKey(true);
                    continue;
                }

                break;
            }

            return (bool) input;
        }

        private static List<Player> InitializePlayers() {
            var playerCount = Rules.GetVal(RuleType.PlayerCount);
            var players = new List<Player>();

            Console.Clear();
            Console.WriteLine($"Create {playerCount} players for the game:");

            for (var i = 0; i < playerCount; i++) {
                string name;

                while (true) {
                    Console.Write($"  - Name for player {i}: ");
                    name = Console.ReadLine();

                    // Check input validity
                    if (string.IsNullOrEmpty(name) || name.Trim().Length == 0) {
                        Console.WriteLine("    - Invalid name!");
                        continue;
                    }
                    
                    // Check name availability
                    if (players.FirstOrDefault(m => m.Name.Equals(name)) != null) {
                        Console.WriteLine("    - Duplicate name!");
                        continue;
                    }

                    break;
                }

                players.Add(new Player(name));
            }

            return players;
        }

        private static bool AutoPlaceShips(Player player) {
            const int tryAmount = 64;
            var boardSize = Rules.GetVal(RuleType.BoardSize);
            var random = new Random();

            foreach (var ship in player.Ships) {
                var placementCount = 0;

                // Attempt to place ship at X different locations
                while (placementCount < tryAmount) {
                    var pos = new Pos(random.Next(0, boardSize), random.Next(0, boardSize));
                    var dir = random.Next(0, 2) == 1 ? ShipDirection.Right : ShipDirection.Down;
                
                    if (!player.CheckIfValidPlacementPos(pos, ship.Size, dir)) {
                        placementCount++;
                        continue;
                    }

                    ship.SetLocation(pos, dir);
                    break;
                }

                if (placementCount >= tryAmount) {
                    return false;
                }
            }

            return true;
        }

        private static void InitializeShips(IEnumerable<Player> players) {
            foreach (var player in players) {
                Console.Clear();
                
                if (AskYesNoQuestion($"Auto place {player.Name}'s ships (y/n): ")) {
                    if (AutoPlaceShips(player)) {
                        BoardGen.GenSingleBoard(player, $"{player.Name}'s board");
                        Console.Write("\nShips automatically placed...");
                        Console.ReadKey(true);
                        continue;
                    }
                    
                    Console.WriteLine("Could not automatically place ships...");
                    Console.ReadKey(true);
                }

                // Manually place ships
                foreach (var ship in player.Ships) {
                    Console.Clear();
                    // Print the player's current board
                    BoardGen.GenSingleBoard(player, $"Place {player.Name}'s ships");
                    Console.WriteLine($"- {ship.Title} (size {ship.Size}):");

                    while (true) {
                        Console.Write("  - x: ");
                        int.TryParse(Console.ReadLine(), out var posX);

                        Console.Write("  - y: ");
                        int.TryParse(Console.ReadLine(), out var posY);

                        Console.Write("  - direction: ");
                        ShipDirection? direction = null;
                        switch (Console.ReadLine()?.ToLower().Trim()) {
                            case "right":
                            case "r":
                                direction = ShipDirection.Right;
                                break;
                            case "down":
                            case "d":
                                direction = ShipDirection.Down;
                                break;
                        }

                        if (direction == null) {
                            Console.WriteLine("Invalid direction! Use (right/r) or (down/d) and try again:");
                            continue;
                        }

                        var pos = new Pos(posX, posY);
                        if (!player.CheckIfValidPlacementPos(pos, ship.Size, (ShipDirection) direction)) {
                            Console.WriteLine("Invalid position! Try again:");
                            continue;
                        }

                        ship.SetLocation(pos, (ShipDirection) direction);
                        break;
                    }
                }
            }
        }

        private static void GameLoop(IReadOnlyList<Player> players) {
            Player winner = null;
            var moves = new List<Move>();
            var turnCount = 0;

            Console.Clear();
            Console.WriteLine("Starting game...");
            Console.ReadKey(true);

            while (true) {
                foreach (var player in players) {
                    // Loop until first alive player is found
                    if (!player.IsAlive()) {
                        continue;
                    }

                    Console.Clear();
                    Console.WriteLine($"It is now {player.Name}'s turn");
                    Console.ReadKey(true);

                    // Find next player in list that is alive
                    var nextPlayer = FindNextPlayer(players, player);

                    Console.Clear();

                    BoardGen.GenTwoBoards(player, nextPlayer);
                    Console.WriteLine($"Turn {turnCount}: {player.Name} is attacking {nextPlayer.Name}");

                    var move = Attack(player, nextPlayer);
                    moves.Add(move);
                    
                    Console.WriteLine($"It was a {move.AttackResult}...");
                    Console.ReadKey(true);

                    // Check if target player is out of the game (all ships have been destroyed)
                    if (!nextPlayer.IsAlive()) {
                        Console.Clear();
                        Console.WriteLine($"{player.Name} knocked {nextPlayer.Name} out of the game!");
                        Console.ReadKey(true);
                    }
                }

                // Check if there is only one player left and therefore the winner of the game
                foreach (var player in players) {
                    if (!player.IsAlive()) {
                        continue;
                    }

                    if (winner == null) {
                        winner = player;
                    } else {
                        winner = null;
                        break;
                    }
                }

                if (winner != null) {
                    break;
                }

                Console.Clear();
                Console.WriteLine($"End of round {turnCount}");
                Console.ReadKey(true);

                turnCount++;
            }

            Console.Clear();
            Console.WriteLine($"The winner of the game is {winner.Name} after {turnCount} turns!");
            Console.ReadKey(true);
        }

        private static Player FindNextPlayer(IReadOnlyList<Player> players, Player player) {
            var playerCount = Rules.GetVal(RuleType.PlayerCount);
            Player nextPlayer = null;

            for (int i = 0; i < playerCount; i++) {
                // Find current player's index
                if (players[i] != player) {
                    continue;
                }

                for (int j = 1; j < playerCount; j++) {
                    var tmpPlayer = players[i + j - (i + j < playerCount ? 0 : playerCount)];

                    if (tmpPlayer.IsAlive()) {
                        nextPlayer = tmpPlayer;
                    }
                }
            }

            // Should not run
            if (nextPlayer == null) {
                throw new NullReferenceException(nameof(nextPlayer));
            }

            return nextPlayer;
        }

        private static Move Attack(Player player, Player targetPlayer) {
            AttackResult result;
            Pos pos;

            while (true) {
                Console.Write("  - x: ");
                int.TryParse(Console.ReadLine(), out var posX);

                Console.Write("  - y: ");
                int.TryParse(Console.ReadLine(), out var posY);

                pos = new Pos(posX, posY);
                result = targetPlayer.AttackAtPos(pos);

                if (result == AttackResult.InvalidAttack) {
                    Console.WriteLine("Invalid position! Try again:");
                    continue;
                }

                if (result == AttackResult.DuplicateAttack) {
                    Console.WriteLine("Already attacked there! Try again:");
                    continue;
                }

                break;
            }

            return new Move(player, targetPlayer, pos, result);
        }
    }
}