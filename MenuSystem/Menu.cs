using Domain;
using Domain.Ship;

namespace MenuSystem {
    public class Menu {
        public Menu() {
            
        }

        public void AskAttackCoords(out bool firstLoop, out int posX, out int posY) {
            firstLoop = false;
            posX = 0;
            posY = 0;
        }
        
        public void AskShipPlacementPosition(out bool firstLoop, out int posX, out int posY, out ShipDirection direction) {
            firstLoop = false;
            posX = 0;
            posY = 0;
            direction = ShipDirection.Right;
        }
        
        public void AskPlayerName(out bool firstLoop, int number, out string name) {
            firstLoop = false;
            name = $"Asd_{number}";
        }
    }
}