using System;

namespace Program {
    class Program {
        static void Main(string[] args) {
            // Give GameSystem access to MenuSystem's menus
            GameSystem.GameLogic.NameMenu = MenuSystem.DynamicMenus.NameInput;
            GameSystem.GameLogic.YesNoQuitMenu = MenuSystem.DynamicMenus.YesOrNoOrQuit;
            GameSystem.GameLogic.YesOrQuitMenu = MenuSystem.DynamicMenus.YesOrQuit;
            GameSystem.GameLogic.AttackCoordMenu = MenuSystem.DynamicMenus.AttackCoords;
            GameSystem.GameLogic.ShipCoordsMenu = MenuSystem.DynamicMenus.ShipCoords;
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Welcome to Battleships - the classical battle ship game!");
            Console.ResetColor();
            
            Console.WriteLine("\nPress any key to start...");
            Console.ReadKey(true);
            
            // Run
            MenuSystem.Menus.MainMenu.RunMenu();
        }
    }
}