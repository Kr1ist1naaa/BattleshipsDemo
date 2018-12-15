using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DomainRule;

namespace MenuSystem {
    public class Menu {
        public string Title { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public List<MenuType> MenuTypes { get; set; } = new List<MenuType>();
        public bool DisplayQuitToMainMenu { get; set; } = false;

        private void PrintMenu() {
            var defaultMenuChoice = MenuItems.FirstOrDefault(m => m.IsDefaultChoice);

            Console.Clear();
            
            Console.Write("========== ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Title);
            Console.ResetColor();
            Console.WriteLine(" ==========");
            
            var titleCharCount = Title.Length;
            var sb = new StringBuilder();
            sb.Insert(0, "=", titleCharCount + 22);

            foreach (var menuItem in MenuItems) {
                if (menuItem.IsDefaultChoice) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(menuItem);
                    Console.ResetColor();
                } else {
                    Console.WriteLine(menuItem);
                }
            }

            Console.WriteLine(sb.ToString());

            Console.WriteLine(Menus.GoBackItem);

            if (DisplayQuitToMainMenu) {
                Console.WriteLine(Menus.QuitToMainItem);
            }

            Console.Write(
                defaultMenuChoice == null ? "> " : "[" + defaultMenuChoice.Shortcut + "]> "
            );
        }

        public string RunMenu() {
            string input;
            MenuItem item;

            while (true) {
                PrintMenu();
                input = Console.ReadLine()?.ToUpper().Trim();

                // Quit menu
                if (input == Menus.GoBackItem.Shortcut) {
                    break;
                }
                
                // Current menu is the rule value input menu
                if (MenuTypes.Contains(MenuType.RuleIntInput)) {
                    // This menu only has one option which will contain the type of rule the user is asked to provide a
                    // value for
                    item = MenuItems.First();

                    // Attempt to parse input as int
                    if (string.IsNullOrEmpty(input) || !int.TryParse(input, out var value)) {
                        Console.WriteLine("Value not an integer!");
                        Console.ReadKey(true);
                        continue;
                    }
                    
                    if (!Rules.ChangeRule(item.RuleTypeToChange, value)) {
                        Console.WriteLine("Value not in range!");
                        Console.ReadKey(true);
                        continue;
                    }

                    Console.WriteLine("Rule value changed!");
                    Console.ReadKey(true);
                    break;
                }

                // Load user-specified or default menuitem
                item = string.IsNullOrWhiteSpace(input)
                    ? MenuItems.FirstOrDefault(m => m.IsDefaultChoice)
                    : MenuItems.FirstOrDefault(m => m.Shortcut == input);

                // The menuitem was null
                if (item == null) {
                    Console.WriteLine("Unknown input!");
                    Console.ReadKey(true);
                    continue;
                }

                // Current menu is the game menu
                if (MenuTypes.Contains(MenuType.GameMenu)) {
                    // Current menu is game loading menu
                    if (MenuTypes.Contains(MenuType.LoadGameMenu) && item.GameId != null) {
                        // Load the game from the database and continue it
                        GameSystem.GameSystem.LoadGame((int) item.GameId);
                        continue;
                    }

                    // Current menu is game deleting menu
                    if (MenuTypes.Contains(MenuType.DeleteGameMenu) && item.GameId != null) {
                        // Delete the game from the database
                        GameSystem.GameSystem.DeleteGame((int) item.GameId);
                        continue;
                    }
                }

                // Current menu is the rules menu
                if (MenuTypes.Contains(MenuType.RulesMenu)) {
                    // Current item has an action
                    if (item.ActionToExecute != null) {
                        item.ActionToExecute();

                        // If current menu item was for resetting some rules, show a confirmation that indeed, the rules
                        // have been set to default values
                        if (item.IsResetRules) {
                            Console.WriteLine("Rules set to default...");
                            Console.ReadKey(true);
                        }
                        
                        continue;
                    }

                    // Current menu is the main rules menu
                    if (item.RuleTypeToChange != null) {
                        DynamicMenus.CreateRunChangeRuleValueMenu(item.RuleTypeToChange);
                        continue;
                    }
                }

                if (item.ActionToExecute != null) {
                    item.ActionToExecute.Invoke();

                    if (item.MenuToRun == null) {
                        continue;
                    }
                }

                // execute the command specified in the menu item
                if (item.MenuToRun == null) {
                    Console.WriteLine(input + " has no command assigned to it!");
                    Console.ReadKey(true);
                    continue;
                }

                // everything should be ok now, lets run it!
                var chosenCommand = item.MenuToRun();
                input = chosenCommand;

                if (MenuTypes[0] != MenuType.MainMenu && chosenCommand == Menus.QuitToMainItem.Shortcut) {
                    break;
                }

                if (chosenCommand != Menus.GoBackItem.Shortcut && chosenCommand != Menus.QuitToMainItem.Shortcut) {
                    Console.ReadKey(true);
                }
            }

            return input;
        }
    }
}