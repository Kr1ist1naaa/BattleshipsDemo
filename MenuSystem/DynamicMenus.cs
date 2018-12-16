using System;
using System.Collections.Generic;
using Domain.DomainRule;
using SaveSystem;

namespace MenuSystem {
    public static class DynamicMenus {
        public static void LoadGame() {
            var menu = new Menu {
                Title = "Load game",
                MenuTypes = new List<MenuType> {MenuType.GameMenu, MenuType.LoadGameMenu},
                MenuItems = new List<MenuItem> ()
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
                MenuItems = new List<MenuItem> ()
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
                Title = "Change rule",
                MenuTypes = new List<MenuType> {MenuType.Input, MenuType.IntInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {
                        Description = $"Enter a value between {rule.MinVal} and {rule.MaxVal}",
                        RuleTypeToChange = ruleType
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
                
                // This menu is guaranteed to return an integer-like string
                var value = int.Parse(input);
                
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

        public static string AskPlayername(int playerNr) {
            var menu = new Menu {
                Title = "Create players",
                MenuTypes = new List<MenuType> {MenuType.Input, MenuType.StringInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {
                        Description = $"Enter a name for player {playerNr}"
                    }
                }
            };

            return menu.RunMenu();
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
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }
}