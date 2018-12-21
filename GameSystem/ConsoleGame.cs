using System;
using System.Collections.Generic;
using BoardUI;
using Domain;
using GameSystem.Logic;

namespace GameSystem {
    public static class ConsoleGame {
        // MenuSystem functions
        public static Func<string, string, string, bool, bool?> YesNoQuitMenu { private get; set; }
        public static Func<string, string, bool> YesOrQuitMenu { private get; set; }
        public static Func<string, string, string> NameMenu { private get; set; }
        public static Func<Player, Player, string[]> AttackCoordMenu { private get; set; }
        public static Func<Player, Ship, string[]> ShipCoordsMenu { private get; set; }

        // SaveSystem functions
        public static Action<int> GameLoader { private get; set; }
        public static Action<int> GameDeleter { private get; set; }
        public static Action GameSaver { private get; set; }

        public static void RunGame() {
            GameLoop();
            
            if (YesOrQuitMenu("Game menu", "Save game")) {
                Console.Clear();
                Console.WriteLine("Saving game...");

                GameSaver();

                Console.Clear();
                Console.WriteLine("Game saved!");
                Console.ReadKey(true);
            }
        }

        public static void LoadGame(int gameId) {
            Console.Clear();
            Console.WriteLine("Loading game...");

            // Load and convert game
            GameLoader(gameId);

            Console.Clear();
            Console.WriteLine("Game loaded!");
            Console.ReadKey(true);
        }

        public static void DeleteGame(int gameId) {
            Console.Clear();
            Console.WriteLine("Deleting game...");

            GameDeleter(gameId);

            Console.Clear();
            Console.WriteLine("Game deleted!");
            Console.ReadKey(true);
        }

        public static void NewGame() {
            // Reset current game to initial state
            ActiveGame.Init();
            
            // Create players
            InitializePlayers();

            // User cancelled players creation and wants to exit
            if (ActiveGame.Players == null) {
                return;
            }

            // Create ships for users
            if (!InitializeShips()) {
                // User cancelled ship creation
                return;
            }

            // Ask to start game
            if (!YesOrQuitMenu("Game menu", "Start game")) {
                return;
            }

            RunGame();
        }

        private static void InitializePlayers() {
            var playerCount = ActiveGame.GetRuleVal(RuleType.PlayerCount);
            ActiveGame.Players = new List<Player>();

            for (var i = 0; i < playerCount; i++) {
                string name;

                while (true) {
                    name = NameMenu("Creating players", $"Input a name for player {i + 1}/{playerCount}");

                    // User chose to quit the menu
                    if (name == null) {
                        return;
                    }

                    // Check input validity
                    if (!InputValidator.CheckValidPlayerName(name)) {
                        Console.WriteLine("Invalid name!");
                        Console.ReadKey(true);
                        continue;
                    }

                    break;
                }

                // Generate ships for the player based on current rules
                var ships = ShipLogic.GenGameShipList();
                ActiveGame.Players.Add(new Player(name, ships));
            }
        }

        private static bool InitializeShips() {
            foreach (var player in ActiveGame.Players) {
                while (true) {
                    // Ask to auto-place ships
                    var t = $"Creating {player.Name}'s ships";
                    var autoPlaceShips = YesNoQuitMenu(t, "Automatically place ships", "Manually place ships", true);

                    // Player requested to quit to main menu
                    if (autoPlaceShips == null) {
                        return false;
                    }

                    if ((bool) autoPlaceShips) {
                        if (!GameLogic.AutoPlaceShips(player)) {
                            PlayerLogic.ResetShips(player);
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

                                var validLoc = InputValidator.CheckValidShipPlacementLoc(player, ship.Size, input[0],
                                    input[1], input[2], out var pos, out var dir);
                                if (!validLoc) {
                                    Console.WriteLine("Invalid location!");
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
                    var accept = YesNoQuitMenu("Ship menu", "Accept positions", "Redo placement", false);

                    // Player requested to quit to main menu
                    if (accept == null) {
                        return false;
                    }

                    if (!(bool) accept) {
                        PlayerLogic.ResetShips(player);
                        continue;
                    }

                    break;
                }
            }

            // Ships were successfully placed
            return true;
        }


        private static void GameLoop() {
            while (true) {
                // Check for any winners
                if (ActiveGame.TrySetWinner()) {
                    break;
                }
                
                // Do the attacks
                foreach (var player in ActiveGame.Players) {
                    // Loop until first alive player is found
                    if (!PlayerLogic.IsAlive(player)) {
                        continue;
                    }

                    Console.Clear();
                    Console.WriteLine($"It is now {player.Name}'s turn");
                    Console.ReadKey(true);

                    // Find next player in list that is alive
                    var nextPlayer = GameLogic.FindNextPlayer(ActiveGame.Players, player);
                    var move = Attack(player, nextPlayer);

                    // User wants to quit the game
                    if (move == null) {
                        return;
                    }

                    ActiveGame.Moves.Add(move);

                    Console.WriteLine($"It was a {move.AttackResult}...");
                    Console.ReadKey(true);

                    // Check if target player is out of the game (all ships have been destroyed)
                    if (!PlayerLogic.IsAlive(nextPlayer)) {
                        Console.Clear();
                        Console.WriteLine($"{player.Name} knocked {nextPlayer.Name} out of the game!");
                        Console.ReadKey(true);
                    }
                }

                Console.Clear();
                Console.WriteLine($"End of round {ActiveGame.RoundCounter}");
                Console.ReadKey(true);

                ActiveGame.RoundCounter++;
            }

            if (ActiveGame.Winner != null) {
                Console.Clear();
                Console.WriteLine(
                    $"The winner of the game is {ActiveGame.Winner.Name} after {ActiveGame.RoundCounter} turns!");
                Console.ReadKey(true);
            }
        }

        private static Move Attack(Player player, Player nextPlayer) {
            while (true) {
                // Ask attack coordinates through MenuSystem
                var input = AttackCoordMenu(player, nextPlayer);

                // User wants to quit the game
                if (input == null) {
                    return null;
                }

                // Check if the specified location can be attacked
                if (!InputValidator.CheckValidAttackLocation(nextPlayer, input[0], input[1], out var pos)) {
                    Console.WriteLine("Invalid attack location!");
                    Console.ReadKey(true);
                    continue;
                }

                // Make the attack
                var result = PlayerLogic.AttackPlayer(nextPlayer, pos);

                // Return whatever
                return new Move(player, nextPlayer, pos, result);
            }
        }
    }
}