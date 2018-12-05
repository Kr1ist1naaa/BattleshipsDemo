using System;

namespace Domain {
    public class Menu {
        private static readonly Random _rnd = new Random();
        private static readonly int boardSize = 10;

        public void AskAttackCoords(out int posX, out int posY) {
            Console.Write("    - x: ");
            int.TryParse(Console.ReadLine(), out posX);

            Console.Write("    - y: ");
            int.TryParse(Console.ReadLine(), out posY);

            //posX = _rnd.Next(0, boardSize);
            //posY = _rnd.Next(0, boardSize);
        }

        public void AskShipPlacementPosition(out int posX, out int posY, out string direction) {
            Console.Write("   - x: ");
            //int.TryParse(Console.ReadLine(), out posX);

            Console.Write("   - y: ");
            //int.TryParse(Console.ReadLine(), out posY);
            
            Console.Write("   - direction (right / down): ");
            //direction = Console.ReadLine();

            posX = _rnd.Next(0, boardSize);
            posY = _rnd.Next(0, boardSize);
            direction = _rnd.Next(0, 2) == 1 ? "right" : "down";
        }

        public void AskPlayerName(int number, out string name) {
            name = $"Asd_{number}";
            //name = Console.ReadLine();
        }

        public bool AskNewGame() {
            return false;
        }

        public void AskBaseRule(Rule rule) {
            Console.Write($"  - {rule.RuleName}: ");
            
            //int.TryParse(Console.ReadLine(), out var val);
            //rule.Value = val;
            
            if (rule.RuleName == Rule.BoardSize.RuleName) {
                rule.Value = 10;
            } else if (rule.RuleName == Rule.PlayerCount.RuleName) {
                rule.Value = 2;
            } else if (rule.RuleName == Rule.ShipCount.RuleName) {
                rule.Value = 5;
            } else if (rule.RuleName == Rule.ShipPadding.RuleName) {
                rule.Value = 1;
            }
        }
    }
}