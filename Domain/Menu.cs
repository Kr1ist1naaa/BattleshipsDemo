using System;

namespace Domain {
    public class Menu {
        private static readonly Random _rnd = new Random();
        private const int boardSize = 10;

        public Menu() {
        }

        public bool AskAttackCoords(bool firstLoop, out int posX, out int posY) {
            //Console.WriteLine(firstLoop ? "Please enter attack pos" : "Please enter <<VALID>> attack pos!");

            posX = _rnd.Next(0, boardSize);
            posY = _rnd.Next(0, boardSize);

            return false;
        }

        public bool AskShipPlacementPosition(bool firstLoop, out int posX, out int posY, out string direction) {
            //Console.WriteLine(firstLoop ? "Please enter ship pos" : "Please enter <<VALID>> ship pos!");

            posX = _rnd.Next(0, boardSize);
            posY = _rnd.Next(0, boardSize);
            direction = _rnd.Next(0, 2) == 1 ? "right" : "down";

            return false;
        }

        public bool AskPlayerName(bool firstLoop, int number, out string name) {
            //Console.WriteLine(firstLoop ? "Please enter player name" : "Please enter <<VALID>> player name!");

            name = $"Asd_{number}";

            return false;
        }

        public bool AskNewGame() {
            return false;
        }

        public bool AskBaseRule(bool firstLoop, Rule rule) {
            if (rule.RuleName == Rule.BoardSize.RuleName) {
                rule.Value = 10;
            } else if (rule.RuleName == Rule.PlayerCount.RuleName) {
                rule.Value = 2;
            } else if (rule.RuleName == Rule.ShipCount.RuleName) {
                rule.Value = 5;
            }

            return false;
        }
    }
}