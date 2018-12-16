using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BoardUI;
using Domain.DomainRule;
using SaveSystem;
using Domain;
using Domain.Ship;

namespace MenuSystem {
    public static class DynamicMenus {
        public static string NameInput(string title, string question) {
            var menu = new Menu {
                Title = title,
                DisableGoBackItem = true,
                DisplayQuitToMainMenu = true,
                MenuTypes = new List<MenuType> {MenuType.Input, MenuType.NameInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {
                        Description = question
                    }
                }
            };

            while (true) {
                var input = menu.RunMenu();
                
                // If user entered go back shortcut
                if (input.ToUpper().Equals(Menus.QuitToMainItem.Shortcut)) {
                    return null;
                }
                
                // Attempt to parse input
                if (string.IsNullOrEmpty(input)) {
                    Console.WriteLine("Invalid input!");
                    Console.ReadKey(true);
                    continue;
                }
                        
                // Would normally be const / static readonly
                if (!new Regex("^[a-zA-Z0-9]*$").IsMatch(input)) {
                    Console.WriteLine("Invalid non-alphanumeric characters!");
                    Console.ReadKey(true);
                    continue;
                }
                
                
                return input;
            }
        }

        public static int[] AttackCoords(Player p1, Player p2) {
            var menu = new Menu {
                Title = "Enter attack coordinates",
                DisableGoBackItem = true,
                DisplayQuitToMainMenu = true,
                ClearConsole = false,
                MenuTypes = new List<MenuType> {MenuType.Input, MenuType.CoordInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {
                        Description = "Input format: <x> <y>"
                    }
                }
            };

            while (true) {
                Console.Clear();
                Console.WriteLine();
                BoardGen.GenTwoBoards(p1, p2);
                Console.WriteLine();

                var input = menu.RunMenu();

                if (input.ToUpper().Equals(Menus.QuitToMainItem.Shortcut)) {
                    return null;
                }

                var split = input.Split(" ");

                if (split.Length != 2) {
                    Console.WriteLine("Invalid number of arguments!");
                    Console.ReadKey(true);
                    continue;
                }

                var strX = split[0].Trim();
                var strY = split[1].Trim();

                if (strX.Length < 1 || strX.Length > 16 || strY.Length < 1 || strY.Length > 16) {
                    Console.WriteLine("Invalid input length!");
                    Console.ReadKey(true);
                    continue;
                }

                if (!int.TryParse(strX, out var x)) {
                    Console.WriteLine("Non-integer input!");
                    Console.ReadKey(true);
                    continue;
                }

                if (!int.TryParse(strY, out var y)) {
                    Console.WriteLine("Non-integer input!");
                    Console.ReadKey(true);
                    continue;
                }

                return new[] {x, y};
            }
        }

        public static int[] ShipCoords(Player player, Ship ship) {
            var menu = new Menu {
                Title = $"Place size {ship.Size} {ship.Title}",
                DisableGoBackItem = true,
                DisplayQuitToMainMenu = true,
                ClearConsole = false,
                MenuTypes = new List<MenuType> {MenuType.Input, MenuType.ShipCoordInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {Description = "Input format: <x> <y> <direction>"},
                    new MenuItem {Description = "<direction> is right/r or down/d"}
                }
            };

            while (true) {
                Console.Clear();
                Console.WriteLine();
                BoardGen.GenSingleBoard(player, $"Place {player.Name}'s ships");
                Console.WriteLine();

                var input = menu.RunMenu();

                if (input.ToUpper().Equals(Menus.QuitToMainItem.Shortcut)) {
                    return null;
                }

                var split = input.Split(" ");

                if (split.Length != 3) {
                    Console.WriteLine("Invalid number of arguments!");
                    Console.ReadKey(true);
                    continue;
                }

                var strX = split[0].Trim();
                var strY = split[1].Trim();
                var strDir = split[2].Trim();

                if (strX.Length < 1 || strX.Length > 16 ||
                    strY.Length < 1 || strY.Length > 16 ||
                    strDir.Length < 1 || strDir.Length > 16) {
                    Console.WriteLine("Invalid input length!");
                    Console.ReadKey(true);
                    continue;
                }

                if (!int.TryParse(strX, out var x)) {
                    Console.WriteLine("Non-integer input!");
                    Console.ReadKey(true);
                    continue;
                }

                if (!int.TryParse(strY, out var y)) {
                    Console.WriteLine("Non-integer input!");
                    Console.ReadKey(true);
                    continue;
                }

                int? dir = null;
                if (strDir.Equals("r") || strDir.Equals("right")) {
                    dir = 0;
                } else if (strDir.Equals("d") || strDir.Equals("down")) {
                    dir = 1;
                }

                if (dir == null) {
                    Console.WriteLine("Invalid direction!");
                    Console.ReadKey(true);
                    continue;
                }

                return new[] {x, y, (int) dir};
            }
        }


        public static bool YesOrQuit(string title, string o) {
            var menu = new Menu {
                Title = title,
                DisableGoBackItem = true,
                DisplayQuitToMainMenu = true,
                MenuTypes = new List<MenuType> {MenuType.Input, MenuType.YesNoInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {
                        Description = o,
                        Shortcut = "1",
                        IsDefaultChoice = true
                    }
                }
            };

            return menu.RunMenu().ToUpper().Equals("1");
        }

        public static bool? YesOrNoOrQuit(string title, string o1, string o2, bool clear) {
            var menu = new Menu {
                Title = title,
                DisableGoBackItem = true,
                DisplayQuitToMainMenu = true,
                ClearConsole = clear,
                MenuTypes = new List<MenuType> {MenuType.Input, MenuType.YesNoInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {
                        Description = o1,
                        Shortcut = "1",
                        IsDefaultChoice = true
                    },
                    new MenuItem {
                        Description = o2,
                        Shortcut = "2"
                    }
                }
            };

            var input = menu.RunMenu().ToUpper();

            if (input.Equals(Menus.QuitToMainItem.Shortcut)) {
                return null;
            }

            return input.Equals("1");
        }

        public static void LoadGame() {
            var menu = new Menu {
                Title = "Load game",
                MenuTypes = new List<MenuType> {MenuType.GameMenu, MenuType.LoadGameMenu},
                MenuItems = new List<MenuItem>()
            };

            Console.Clear();
            Console.WriteLine("Loading game list...");

            // Get list of saved games
            var saves = GameSaver.GetSaveGameList();
            var counter = 1;

            // Create a menuitem for each saved game
            foreach (var save in saves) {
                int.TryParse(save[0], out var gameId);
                var saveTime = save[2];
                var turnCount = save[1];

                // Create a menuitem for the save game
                var menuItem = new MenuItem {
                    Description = $"{saveTime} (turn {turnCount,3})",
                    GameId = gameId,
                    Shortcut = counter.ToString()
                };

                // Add it to the menu
                menu.MenuItems.Add(menuItem);
                counter++;
            }

            // Run the generated menu
            menu.RunMenu();
        }

        public static void DeleteGame() {
            var menu = new Menu {
                Title = "Delete game",
                MenuTypes = new List<MenuType> {MenuType.GameMenu, MenuType.DeleteGameMenu},
                MenuItems = new List<MenuItem>()
            };

            Console.Clear();
            Console.WriteLine("Loading game list...");

            // Get list of saved games
            var saves = GameSaver.GetSaveGameList();
            var counter = 1;

            // Create a menuitem for each saved game
            foreach (var save in saves) {
                int.TryParse(save[0], out var gameId);
                var saveTime = save[2];
                var turnCount = save[1];

                // Create a menuitem for the save game
                var menuItem = new MenuItem {
                    Description = $"{saveTime} (turn {turnCount,3})",
                    GameId = gameId,
                    Shortcut = counter.ToString()
                };

                // Add it to the menu
                menu.MenuItems.Add(menuItem);
                counter++;
            }

            // Run the generated menu
            menu.RunMenu();
        }

        public static void ChangeRuleValue(RuleType? ruleType) {
            var rule = Rules.GetRule(ruleType);

            var menu = new Menu {
                Title = $"Change rule {rule.RuleType}",
                MenuTypes = new List<MenuType> {MenuType.Input, MenuType.RuleIntInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {
                        Description = $"Current value: {rule.Value}"
                    },
                    new MenuItem {
                        Description = $"Enter a value between {rule.MinVal} and {rule.MaxVal}"
                    }
                }
            };

            while (true) {
                // Run menu, ask user for integer input
                var input = menu.RunMenu();

                // User entered exit shortcut
                if (input.ToUpper() == Menus.GoBackItem.Shortcut) {
                    return;
                }
                
                // Attempt to parse input as int
                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var value)) {
                    Console.WriteLine("Value not an integer!");
                    Console.ReadKey(true);
                    continue;
                }

                if (!Rules.ChangeRule(ruleType, value)) {
                    Console.WriteLine("Value not in range!");
                    Console.ReadKey(true);
                    continue;
                }

                Console.WriteLine("Rule value changed!");
                Console.ReadKey(true);
                break;
            }
        }

        public static void PrintAllRules() {
            Console.Clear();
            Console.WriteLine("The active rules are:");

            foreach (var rule in Rules.RuleSet) {
                Console.Write($"  - {rule.RuleType,-16}: ");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{rule.Value,3}");
                Console.ResetColor();

                Console.WriteLine($" ({rule.MinVal} .. {rule.MaxVal,3})");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}