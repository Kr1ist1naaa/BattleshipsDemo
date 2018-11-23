using System;

namespace MenuSystem {
    public class Menu {
        private readonly Random _rnd = new Random(45665465);
        
        public Menu() {
            
        }

        public bool AskAttackCoords(bool firstLoop, out int posX, out int posY) {
            Console.WriteLine(firstLoop ? "Please enter attack pos" : "Please enter <<VALID>> attack pos!");
            
            posX = _rnd.Next(0, 10);
            posY = _rnd.Next(0, 10);
            
            return false;
        }
        
        public bool AskShipPlacementPosition(bool firstLoop, out int posX, out int posY, out char direction) {
            //Console.WriteLine(firstLoop ? "Please enter ship pos" : "Please enter <<VALID>> ship pos!");
            
            posX = _rnd.Next(0, 10);
            posY = _rnd.Next(0, 10);
            direction = _rnd.Next(0, 2) == 1 ? 'r' : 'd';
            
            return false;
        }
        
        public bool AskPlayerName(bool firstLoop, int number, out string name) {
            //Console.WriteLine(firstLoop ? "Please enter player name" : "Please enter <<VALID>> player name!");

            name = $"Asd_{number}";
            
            return false;
        }
        
        public bool AskGameDetails(bool firstLoop, out int playerCount, out int boardSizeX, out int boardSizeY) {
            Console.WriteLine(firstLoop ? "Please enter game details" : "Please enter <<VALID>> game details!");

            playerCount = 2;
            boardSizeX = 10;
            boardSizeY = 10;
            
            return false;
        }
        
        public bool AskNewGame() {
            return false;
        }
    }
}