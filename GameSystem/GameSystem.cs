using System;
using System.Collections.Generic;
using System.Linq;
using BoardUI;
using Domain;
using Domain.DomainRule;
using Domain.Ship;
using SaveSystem;

namespace GameSystem {
    public static class GameSystem {
        public static void LoadGame() {
            Console.Clear();
            Console.WriteLine("Loading game list...");

            var saves = GameSaver.GetSaveGameList();
            
            Console.Clear();
            Console.WriteLine("Saves:");
            
            var indexMappings = new Dictionary<int, int>();
            var counter = 1;
            
            foreach (var save in saves) {
                Console.Write("  - [");
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{counter}");
                Console.ResetColor();
                
                Console.WriteLine($"] {save[2]} (turn {save[1],3})");
                
                int.TryParse(save[0], out var saveId);
                indexMappings.Add(counter, saveId);
                
                counter++;
            }

            int? gameIndex;
            
            while (true) {
                gameIndex = AskNumericInputOrCancel("Enter game number to load: ");
                if (gameIndex == null) {
                    return;
                }

                if (indexMappings.ContainsKey((int) gameIndex)) {
                    break;
                }

                Console.WriteLine("  - Invalid game number");
            }

            var game = GameSaver.Load(indexMappings[(int)gameIndex]);
            
            Console.Clear();
            Console.WriteLine("Game loaded!");
            Console.ReadKey(true);
            
            GameLoop(game);
        }

        private static void ReplayGame() {
            // todo: this
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
            
            Console.Clear();
            Console.WriteLine("Starting game...");
            Console.ReadKey(true);

            var game = new Domain.Game(players);
            GameLoop(game);
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
                    Console.WriteLine("Unknown input...");
                    Console.ReadKey(true);
                    continue;
                }

                break;
            }

            return (bool) input;
        }

        private static int? AskNumericInputOrCancel(string question, int indent = 2) {
            while (true) {
                Console.Write(question);

                var input = Console.ReadLine()?.ToLower().Trim();
                
                switch (input) {
                    case "x":
                    case "q":
                    case "quit":
                    case "exit":
                        return null;
                }
                
                if (!int.TryParse(input, out var newValue)) {
                    Console.Write(new string(' ', indent));
                    Console.WriteLine("- Invalid input!");
                    continue;
                }

                return newValue;
            }
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

        private static void GameLoop(Domain.Game game) {
            while (true) {
                // Do the attacks
                foreach (var player in game.Players) {
                    // Loop until first alive player is found
                    if (!player.IsAlive()) {
                        continue;
                    }

                    Console.Clear();
                    Console.WriteLine($"It is now {player.Name}'s turn");
                    Console.ReadKey(true);

                    // Find next player in list that is alive
                    var nextPlayer = FindNextPlayer(game.Players, player);

                    Console.Clear();

                    BoardGen.GenTwoBoards(player, nextPlayer);
                    Console.WriteLine($"Turn {game.TurnCount}: {player.Name} is attacking {nextPlayer.Name}");

                    var move = Attack(player, nextPlayer);
                    game.Moves.Add(move);

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
                foreach (var player in game.Players) {
                    if (!player.IsAlive()) {
                        continue;
                    }

                    if (game.Winner == null) {
                        game.Winner = player;
                    } else {
                        game.Winner = null;
                        break;
                    }
                }

                if (game.Winner != null) {
                    break;
                }

                Console.Clear();
                Console.WriteLine($"End of round {game.TurnCount}");
                if (!AskYesNoQuestion("Continue with game (y/n): ")) {
                    Console.Clear();
            
                    if (AskYesNoQuestion("Save game (y/n): ")) {
                        if (game.GameId == null) {
                            GameSaver.Save(game);
                        } else {
                            GameSaver.OverwriteSave(game);
                        }

                        Console.Clear();
                        Console.WriteLine("Game saved...");
                        Console.ReadKey(true);
                        break;
                    }
                }

                game.TurnCount++;
            }

            if (game.Winner != null) {
                Console.Clear();
                Console.WriteLine($"The winner of the game is {game.Winner.Name} after {game.TurnCount} turns!");
                Console.ReadKey(true);
            }
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
                var posX = AskNumericInputOrCancel("  - x: ", 4);
                if (posX == null) {
                    return null;
                }
                
                var posY = AskNumericInputOrCancel("  - y: ", 4);
                if (posY == null) {
                    return null;
                }

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