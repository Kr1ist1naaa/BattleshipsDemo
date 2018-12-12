using System;

namespace Domain {
    public class Menu {
        private static readonly Random _rnd = new Random();
        private static readonly int boardSize = 6;

        public void AskAttackCoords(out int posX, out int posY) {
            posX = _rnd.Next(0, boardSize);
            posY = _rnd.Next(0, boardSize);
            
            //Console.Write("    - x: ");
            //int.TryParse(Console.ReadLine(), out posX);
            Console.WriteLine($"  - x: {posX}");

            //Console.Write("    - y: ");
            //int.TryParse(Console.ReadLine(), out posY);
            Console.WriteLine($"  - y: {posY}");
        }

        public void AskShipPlacementPosition(out int posX, out int posY, out string direction) {
            posX = _rnd.Next(0, boardSize);
            posY = _rnd.Next(0, boardSize);
            direction = _rnd.Next(0, 2) == 1 ? "right" : "down";
            
            //Console.Write("  - x: ");
            //int.TryParse(Console.ReadLine(), out posX);
            Console.WriteLine($"  - x: {posX}");

            //Console.Write("  - y: ");
            //int.TryParse(Console.ReadLine(), out posY);
            Console.WriteLine($"  - y: {posY}");
            
            //Console.Write("  - direction (right / down): ");
            //direction = Console.ReadLine();
            Console.WriteLine($"  - direction (right / down): {direction}");
        }

        public void AskPlayerName(int number, out string name) {
            //Console.Write("    - name: ");
            //name = Console.ReadLine();
            
            name = $"Asd_{number}";
            Console.WriteLine($"    - name: {name}");
        }


    }
}