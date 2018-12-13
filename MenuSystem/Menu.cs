using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Rule;
using Game = GameSystem.Game;

namespace MenuSystem {
    public class Menu {
        public string Title { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public List<MenuType> MenuTypes { get; set; } = new List<MenuType>();
        public bool DisplayQuitToMainMenu { get; set; } = false;

        private void PrintMenu() {
            var defaultMenuChoice = MenuItems.FirstOrDefault(m => m.IsDefaultChoice);

            Console.Clear();
            Console.WriteLine($"=========={Title}==========");
            var titleCharCount = Title.Length;
            var sb = new StringBuilder();
            sb.Insert(0, "=", titleCharCount + 20);

            foreach (var menuItem in MenuItems) {
                if (menuItem.IsDefaultChoice) {
                    Console.ForegroundColor =
                        ConsoleColor.Red;
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
            bool done;
            string input;
            
            do {
                done = false;

                PrintMenu();
                input = Console.ReadLine()?.ToUpper().Trim();

                // shall we exit from this menu
                if (input == Menus.GoBackItem.Shortcut) {
                    break;
                }

                if (DisplayQuitToMainMenu && input == Menus.QuitToMainItem.Shortcut) {
                    break; // jump out of the loop
                }

                var item = string.IsNullOrWhiteSpace(input)
                    ? MenuItems.FirstOrDefault(m => m.IsDefaultChoice)
                    : MenuItems.FirstOrDefault(m => m.Shortcut == input);

                if (item == null) {
                    Console.WriteLine("Not found in the list of commands!");
                    Console.ReadKey(true);
                    continue;
                }

                // User chose to set a rule value
                if (MenuTypes[0] == MenuType.RulesMenu  && item.RuleType != null) {
                    ChangeRuleValue(item.RuleType);

                    Console.WriteLine("Rule changed");
                    Console.ReadKey(true);
                    continue;
                }

                // User chose to rest rules 
                if (MenuTypes[0] == MenuType.RulesMenu && item.ActionToExecute != null) {
                    item.ActionToExecute();
                    
                    switch (MenuTypes[1]) {
                        case MenuType.MainRulesMenu:
                            Console.WriteLine("All rules set to default");
                            break;
                        case MenuType.GeneralRulesMenu:
                            Console.WriteLine("General rules set to default");
                            break;
                        case MenuType.ShipCountRulesMenu:
                            Console.WriteLine("Ship count rules set to default");
                            break;
                        case MenuType.ShipSizeRulesMenu:
                            Console.WriteLine("Ship size rules set to default");
                            break;
                    }
                    
                    Console.ReadKey(true);
                    continue;
                }
                
                // New/load game
                if (MenuTypes[0] == MenuType.GameMenu) {
                    item.ActionToExecute?.Invoke();
                    continue;
                }

                // execute the command specified in the menu item
                if (item.CommandToExecute == null) {
                    Console.WriteLine(input + " has no command assigned to it!");
                    Console.ReadKey(true);
                    continue; // jump back to the start of loop
                }

                // everything should be ok now, lets run it!
                var chosenCommand = item.CommandToExecute();
                input = chosenCommand;

                if (MenuTypes[0] != MenuType.MainMenu && chosenCommand == Menus.QuitToMainItem.Shortcut) {
                    break;
                }

                if (chosenCommand != Menus.GoBackItem.Shortcut && chosenCommand != Menus.QuitToMainItem.Shortcut) {
                    Console.ReadKey(true);
                }

            } while (done == false);

            return input;
        }

        private static void ChangeRuleValue(RuleType? ruleType) {
            Console.Clear();

            var rule = Rules.GetRule(ruleType);
            
            Console.WriteLine(rule.Description);
            Console.WriteLine($"Current value is {rule.Value}");

            while (true) {
                Console.WriteLine($"Enter new value between {rule.MinVal} and {rule.MaxVal}: ");
                Console.Write("> ");
                
                var value = Console.ReadLine();
                
                if (!int.TryParse(value, out var newValue)) {
                    Console.WriteLine("Try again.\n");
                    continue;
                }

                if (!Rules.ChangeRule(ruleType, newValue)) {
                    Console.WriteLine("Try again.\n");
                    continue;
                }

                break;
            }
        }
    }
}