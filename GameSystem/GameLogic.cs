using System;
using System.Collections.Generic;
using System.Linq;
using BoardUI;
using Domain;
using Domain.DomainRule;
using Domain.Ship;
using SaveSystem;

namespace GameSystem {
    public static class GameLogic {
        public static Func<string, string, string, bool, bool?> YesNoQuitMenu { get; set; }
        public static Func<string, string, bool> YesOrQuitMenu { get; set; }
        public static Func<string, string, string> NameMenu { get; set; }
        public static Func<Player, Player, string[]> AttackCoordMenu { get; set; }
        public static Func<Player, Ship, string[]> ShipCoordsMenu { get; set; }


        public static void LoadGame(int gameId) {
            Console.Clear();
            Console.WriteLine("Loading game...");

            // Load and convert game
            var game = GameSaver.Load(gameId);

            // Reset current base mappings
            BaseConversion.Reset();
            
            Console.Clear();
            Console.WriteLine("Game loaded!");
            Console.ReadKey(true);

            GameLoop(game);

            // Ask to save the game
            AskSaveGame(game);
        }

        public static void DeleteGame(int gameId) {
            Console.Clear();
            Console.WriteLine("Deleting game...");

            GameSaver.Delete(gameId);

            Console.Clear();
            Console.WriteLine("Game deleted!");
            Console.ReadKey(true);
        }

        public static void NewGame() {
            // Reset current base mappings
            BaseConversion.Reset();
            
            // Create players
            var players = InitializePlayers();

            // User cancelled players creation
            if (players == null) {
                return;
            }

            // Create ships for users
            if (!InitializeShips(players)) {
                // User cancelled ship creation
                return;
            }

            // Ask to start game
            const string t = "Game menu";
            const string o = "Start game";
            var startGame = YesOrQuitMenu(t, o);
            if (!startGame) {
                return;
            }

            var game = new Game(players);
            GameLoop(game);

            // Ask to save the game
            AskSaveGame(game);
        }

        private static void AskSaveGame(Game game) {
            if (YesOrQuitMenu("Game menu", "Save game")) {
                Console.Clear();
                Console.WriteLine("Saving game...");
                
                if (game.GameId == null) {
                    GameSaver.Save(game);
                } else {
                    GameSaver.OverwriteSave(game);
                }

                Console.Clear();
                Console.WriteLine("Game saved!");
                Console.ReadKey(true);
            }
        }


        private static List<Player> InitializePlayers() {
            var playerCount = Rules.GetVal(RuleType.PlayerCount);
            var players = new List<Player>();

            for (var i = 0; i < playerCount; i++) {
                string name;

                while (true) {
                    name = NameMenu("Creating players", $"Input a name for player {i + 1}/{playerCount}");

                    // User chose to quit the menu
                    if (name == null) {
                        return null;
                    }

                    // Check input validity
                    if (string.IsNullOrEmpty(name) || name.Trim().Length == 0) {
                        Console.WriteLine("Invalid name!");
                        Console.ReadKey(true);
                        continue;
                    }

                    // Check name availability
                    if (players.FirstOrDefault(m => m.Name.Equals(name)) != null) {
                        Console.WriteLine("Name already in use!");
                        Console.ReadKey(true);
                        continue;
                    }

                    break;
                }

                players.Add(new Player(name));
            }

            return players;
        }

        private static bool InitializeShips(IEnumerable<Player> players) {
            foreach (var player in players) {
                while (true) {
                    // Ask to auto-place ships
                    var t = $"Creating {player.Name}'s ships";
                    const string o1 = "Automatically place ships";
                    const string o2 = "Manually place ships";
                    var autoPlaceShips = YesNoQuitMenu(t, o1, o2, true);

                    // Player requested to quit to main menu
                    if (autoPlaceShips == null) {
                        return false;
                    }

                    if ((bool) autoPlaceShips) {
                        if (!AutoPlaceShips(player)) {
                            player.ResetShips();
                            Console.WriteLine("Could not place ships...");
                            Console.ReadKey(true);
                        }
                    } else {
                        // Manually place ships
                        foreach (var ship in player.Ships) {
                            while (true) {
                                var input = ShipCoordsMenu(player, ship);

                                // Player wants to quit game
                                if (input == null) {
                                    return false;
                                }

                                // For the sake of clarity
                                var strX = input[0];
                                var strY = input[1];
                                var strDir = input[2];
                                
                                // Convert dir to enum
                                ShipDirection dir;
                                if (strDir.Equals("r") || strDir.Equals("right")) {
                                    dir = ShipDirection.Right;
                                } else if (strDir.Equals("d") || strDir.Equals("down")) {
                                    dir = ShipDirection.Down;
                                } else {
                                    // Unknown direction specified
                                    Console.WriteLine("Invalid direction!");
                                    Console.ReadKey(true);
                                    continue;
                                }

                                // Check if x coordinate is valid and alphabetic
                                if (BaseConversion.MapToBase10(strX) == null) {
                                    Console.WriteLine("Invalid X coordinate!");
                                    Console.ReadKey(true);
                                    continue;
                                }
                                
                                // Check if y coordinate is a valid integer
                                if (!int.TryParse(strY, out var intY)) {
                                    Console.WriteLine("Invalid Y coordinate!");
                                    Console.ReadKey(true);
                                    continue;
                                }
                                
                                // Convert X coordinate to an integer
                                var intX = (int) BaseConversion.MapToBase10(strX);
                                
                                // Take board orientation and numbering offset into account
                                var x = intX;
                                var y = intY - 1;

                                var pos = new Pos(x, y);

                                if (!player.CheckIfValidPlacementPos(pos, ship.Size, dir)) {
                                    Console.WriteLine("Invalid position! Try again:");
                                    Console.ReadKey(true);
                                    continue;
                                }

                                ship.SetLocation(pos, dir);
                                break;
                            }
                        }
                    }
                    
                    Console.Clear();
                    BoardGen.GenSingleBoard(player, $"{player.Name}'s board");
                    Console.WriteLine();

                    // Ask to re-place the ships
                    var accept = YesNoQuitMenu("Ship menu", "Accept positions", "Redo placement",false);

                    // Player requested to quit to main menu
                    if (accept == null) {
                        return false;
                    }

                    if (!(bool) accept) {
                        player.ResetShips();
                        continue;
                    }

                    break;
                }
            }

            // Ships were successfully placed
            return true;
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


        private static void GameLoop(Game game) {
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
                    var move = Attack(player, nextPlayer);

                    // User wants to quit the game
                    if (move == null) {
                        return;
                    }

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
                Console.ReadKey(true);

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

        private static Move Attack(Player player, Player nextPlayer) {
            while (true) {
                // Ask attack coordinates through MenuSystem
                var input = AttackCoordMenu(player, nextPlayer);

                // User wants to quit the game
                if (input == null) {
                    return null;
                }
                
                // For the sake of clarity
                var strX = input[0];
                var strY = input[1];
                
                // Check if x coordinate is valid and alphabetic
                if (BaseConversion.MapToBase10(strX) == null) {
                    Console.WriteLine("Invalid X coordinate!");
                    Console.ReadKey(true);
                    continue;
                }
                                
                // Check if y coordinate is a valid integer
                if (!int.TryParse(strY, out var intY)) {
                    Console.WriteLine("Invalid Y coordinate!");
                    Console.ReadKey(true);
                    continue;
                }
                                
                // Convert X coordinate to an integer
                var intX = (int) BaseConversion.MapToBase10(strX);
                                
                // Take board orientation and numbering offset into account
                var x = intY - 1;
                var y = intX;

                var pos = new Pos(x, y);
                var result = nextPlayer.AttackAtPos(pos);

                if (result == AttackResult.InvalidAttack) {
                    Console.WriteLine("Invalid position!");
                    Console.ReadKey(true);
                    continue;
                }

                if (result == AttackResult.DuplicateAttack) {
                    Console.WriteLine("Already attacked there!");
                    Console.ReadKey(true);
                    continue;
                }

                return new Move(player, nextPlayer, pos, result);
            }
        }
    }
}