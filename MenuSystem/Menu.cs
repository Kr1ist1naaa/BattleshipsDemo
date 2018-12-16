using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MenuSystem {
    public class Menu {
        public string Title { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public List<MenuType> MenuTypes { get; set; }
        public bool DisplayQuitToMainMenu { get; set; }
        public bool DisableGoBackItem { get; set; }
        public bool ClearConsole { get; set; } = true;

        private void PrintMenu() {
            if (ClearConsole) {
                Console.Clear();
            }

            Console.Write("========== ");
            Console.ForegroundColor = ConsoleColor.Red;
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

            if (!DisableGoBackItem) {
                if (MenuTypes.Contains(MenuType.MainMenu)) {
                    Console.WriteLine(Menus.ExitProgramItem);
                } else {
                    Console.WriteLine(Menus.GoBackItem);
                }
            }

            if (DisplayQuitToMainMenu) {
                Console.WriteLine(Menus.QuitToMainItem);
            }

            Console.Write("> ");
        }

        public string RunMenu() {
            string rawInput, input;
            MenuItem item;

            while (true) {
                PrintMenu();
                
                rawInput = Console.ReadLine()?.Trim();
                input = rawInput?.ToUpper();
                
                // Quit to main menu
                if (DisplayQuitToMainMenu && input == Menus.QuitToMainItem.Shortcut) {
                    break;
                }

                // Go back
                if (!DisableGoBackItem && input == Menus.GoBackItem.Shortcut) {
                    break;
                }
                
                // Current menu contains no shortcuts and is only for inputting a value
                if (MenuTypes.Contains(MenuType.Input)) {
                    return rawInput;
                }

                // Load user-specified or default menuitem
                item = string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input)
                    ? MenuItems.FirstOrDefault(m => m.IsDefaultChoice)
                    : MenuItems.FirstOrDefault(m => m.Shortcut == input);

                // The menuitem was null
                if (item == null) {
                    Console.WriteLine("Unknown input!");
                    Console.ReadKey(true);
                    continue;
                }
                
                // Again the input but this time the user can actually cancel and go back
                if (MenuTypes.Contains(MenuType.Input)) {
                    if (MenuTypes.Contains(MenuType.YesNoInput)) {
                        return item.Shortcut;
                    }
                }

                // Current menu is the game menu
                if (MenuTypes.Contains(MenuType.GameMenu)) {
                    // Current menu is game loading menu
                    if (MenuTypes.Contains(MenuType.LoadGameMenu) && item.GameId != null) {
                        // Load the game from the database and continue it
                        GameSystem.GameLogic.LoadGame((int) item.GameId);
                        
                        // Return the GoBackItem shortcut to exit the game loading menu
                        return Menus.GoBackItem.Shortcut;
                    }

                    // Current menu is game deleting menu
                    if (MenuTypes.Contains(MenuType.DeleteGameMenu) && item.GameId != null) {
                        // Delete the game from the database
                        GameSystem.GameLogic.DeleteGame((int) item.GameId);
                        
                        // Return the GoBackItem shortcut to exit the game deletion menu
                        return Menus.GoBackItem.Shortcut;
                    }
                    
                    // Current menu is new game menu
                    if (MenuTypes.Contains(MenuType.NewGameMenu)) {
                        // Start new game
                        if (item.ActionToExecute != null) {
                            item.ActionToExecute();

                            // Return the GoBackItem shortcut to exit the menu
                            return Menus.GoBackItem.Shortcut;
                        }
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
                        DynamicMenus.ChangeRuleValue(item.RuleTypeToChange);
                        continue;
                    }
                }

                if (item.ActionToExecute != null) {
                    item.ActionToExecute();

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
                input = item.MenuToRun();

                if (!MenuTypes.Contains(MenuType.MainMenu) && input == Menus.QuitToMainItem.Shortcut) {
                    break;
                }

                if (input != Menus.GoBackItem.Shortcut && input != Menus.QuitToMainItem.Shortcut) {
                    Console.ReadKey(true);
                }
            }

            return input;
        }
    }
}