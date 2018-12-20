using System;

namespace Program {
    class Program {
        static void Main(string[] args) {
            // Give GameSystem access to MenuSystem's menus
            GameSystem.ConsoleGame.NameMenu = MenuSystem.DynamicMenus.NameInput;
            GameSystem.ConsoleGame.YesNoQuitMenu = MenuSystem.DynamicMenus.YesOrNoOrQuit;
            GameSystem.ConsoleGame.YesOrQuitMenu = MenuSystem.DynamicMenus.YesOrQuit;
            GameSystem.ConsoleGame.AttackCoordMenu = MenuSystem.DynamicMenus.AttackCoords;
            GameSystem.ConsoleGame.ShipCoordsMenu = MenuSystem.DynamicMenus.ShipCoords;
            
            // Tie SaveSystem functions 
            GameSystem.ConsoleGame.GameSaver = SaveSystem.GameSaver.Save;
            GameSystem.ConsoleGame.GameLoader = SaveSystem.GameSaver.Load;
            GameSystem.ConsoleGame.GameDeleter = SaveSystem.GameSaver.Delete;

            BoardUI.BoardGen.GetShipOrNull = GameSystem.Logic.PlayerLogic.GetShipOrNull;
            BoardUI.BoardGen.IsShipDestroyed = GameSystem.Logic.ShipLogic.IsDestroyed;
            BoardUI.BoardGen.MapToBase26 = GameSystem.BaseConversion.MapToBase26;
            BoardUI.BoardGen.GetRuleVal = GameSystem.ActiveGame.GetRuleVal;
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Welcome to Battleships - the classical battle ship game!");
            Console.ResetColor();
            
            Console.WriteLine("\nPress any key to start...");
            Console.ReadKey(true);
            
            // Run
            MenuSystem.MenuInitializers.MainMenu.RunMenu();
        }
    }
}