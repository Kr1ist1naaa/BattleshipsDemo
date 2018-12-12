using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Domain.Rule;

namespace MenuSystem {
    public class Menu {
        public string Title { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        private MenuItem GoBackItem { get; set; } = new MenuItem {
            Shortcut = "X",
            Description = "Go back!"
        };

        private MenuItem QuitToMainItem { get; set; } = new MenuItem {
            Shortcut = "Q",
            Description = "Quit to main menu!"
        };

        public bool DisplayQuitToMainMenu { get; set; } = false;
        public bool IsMainMenu { get; set; } = false;
        public bool IsGameMenu { get; set; } = false;
        public bool IsRulesMenu { get; set; } = false;

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

            Console.WriteLine(GoBackItem);

            if (DisplayQuitToMainMenu) {
                Console.WriteLine(QuitToMainItem);
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
                if (input == GoBackItem.Shortcut) {
                    break;
                }

                if (DisplayQuitToMainMenu && input == QuitToMainItem.Shortcut) {
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

                if (IsRulesMenu) {
                    if (item.RuleType.Equals(RuleType.ResetDefault)) {
                        Rules.ResetDefault();
                    } else {
                        OptionChangingFromMenu(item.RuleType);
                    }

                    Console.WriteLine("Option changed");
                    Console.ReadKey(true);
                    continue;
                }

                if (IsGameMenu) {
                    if (item.Shortcut.Equals("A")) {
                        //Game.FullGame();
                        continue;
                    }

                    if (item.Shortcut.Equals("B")) {
                        //Game.SelectGameAndStart();
                        continue;
                    }
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

                if (IsMainMenu == false && chosenCommand == QuitToMainItem.Shortcut) {
                    break;
                }

                if (chosenCommand != GoBackItem.Shortcut && chosenCommand != QuitToMainItem.Shortcut) {
                    Console.ReadKey(true);
                }

            } while (done != true);


            return input;
        }


        private static void OptionChangingFromMenu(RuleType ruleType) {
            Console.Clear();

            var rule = Rules.GetRule(ruleType);
            Console.WriteLine($"Min val is {rule.MinVal}, max is {rule.MaxVal}");
            
            while (true) {
                Console.WriteLine($"Changing option: {rule.RuleType} | Current value: {rule.Value}");
                Console.WriteLine("Enter new value: ");
                
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