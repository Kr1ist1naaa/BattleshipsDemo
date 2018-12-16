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
        public static Func<string, string, string, bool?> YesNoQuitMenu { get; set; }
        public static Func<string, string, bool> YesOrQuitMenu { get; set; }
        public static Func<string, string, string> NameMenu { get; set; }
        public static Func<Player, Player, int[]> AttackCoordMenu { get; set; }
        public static Func<Player, Ship, int[]> ShipCoordsMenu { get; set; }

        public static void LoadGame(int gameId) {
            Console.Clear();
            Console.WriteLine("Loading game...");

            var game = GameSaver.Load(gameId);

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
                if (game.GameId == null) {
                    GameSaver.Save(game);
                } else {
                    GameSaver.OverwriteSave(game);
                }

                Console.Clear();
                Console.WriteLine("Game saved...");
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
                    var autoPlaceShips = YesNoQuitMenu(t, o1, o2);

                    // Player requested to quit to main menu
                    if (autoPlaceShips == null) {
                        return false;
                    }

                    if ((bool) autoPlaceShips) {
                        if (!AutoPlaceShips(player)) {
                            player.ResetShips();
                            Console.Write("Could not place ships...");
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

                                var pos = new Pos(input[0], input[1]);
                                var dir = (ShipDirection) input[2];

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
                    Console.WriteLine("\nAll ships placed!");
                    Console.ReadKey(true);

                    // Ask to re-place the ships
                    var replaceShips = YesNoQuitMenu("Ship menu", "Accept positions", "Redo placement");

                    // Player requested to quit to main menu
                    if (replaceShips == null) {
                        return false;
                    }

                    if ((bool) replaceShips) {
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

                var pos = new Pos(input[0], input[1]);
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