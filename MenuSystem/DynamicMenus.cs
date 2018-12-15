using System;
using System.Collections.Generic;
using Domain.DomainRule;
using SaveSystem;

namespace MenuSystem {
    public static class DynamicMenus {
        public static void CreateRunLoadGameMenu() {
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
        
        public static void CreateRunDeleteGameMenu() {
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
        
        public static void CreateRunChangeRuleValueMenu(RuleType? ruleType) {
            var rule = Rules.GetRule(ruleType);
            
            var menu = new Menu {
                Title = "Change rule",
                MenuTypes = new List<MenuType> {MenuType.RuleIntInput},
                MenuItems = new List<MenuItem> {
                    new MenuItem {
                        Description = $"Enter a value between {rule.MinVal} and {rule.MaxVal}",
                        RuleTypeToChange = ruleType
                    }
                }
            };

            menu.RunMenu();
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