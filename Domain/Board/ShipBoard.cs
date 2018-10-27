using Domain.Ship;

namespace Domain.Board {
    public class ShipBoard {
        private uint SizeX {get;}
        private uint SizeY {get;}
        private Ship.Ship[][] Board { get; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardSizeX"></param>
        /// <param name="boardSizeY"></param>
        public ShipBoard(uint boardSizeX, uint boardSizeY) {
            SizeX = boardSizeX;
            SizeY = boardSizeY;
            
            Board = new Ship.Ship[SizeX][];
            for (var i = 0; i < SizeX; i++) {
                Board[i] = new Ship.Ship[SizeY];
            }
        }

        /// <summary>
        /// Places a ship on the board
        /// </summary>
        /// <param name="boardPosX"></param>
        /// <param name="boardPosY"></param>
        /// <param name="ship"></param>
        /// <param name="shipDirection"></param>
        /// <returns>True if ship was be placed, false if not</returns>
        public bool PlaceShip(uint boardPosX, uint boardPosY, Ship.Ship ship, ShipDirection shipDirection) {
            var check = CheckPlacementSpot(boardPosX, boardPosY, ship.Size, shipDirection);
            if (!check) return false;

            for (int i = 0; i < ship.Size; i++) {
                if (shipDirection == ShipDirection.Right) {
                    Board[boardPosX + i][boardPosY] = ship;
                } else {
                    Board[boardPosX][boardPosY + i] = ship;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the board is free at the specified location
        /// </summary>
        /// <param name="boardPosX"></param>
        /// <param name="boardPosY"></param>
        /// <param name="shipSize"></param>
        /// <param name="shipDirection"></param>
        /// <returns>True if ship can be placed, false if cannot.</returns>
        private bool CheckPlacementSpot(uint boardPosX, uint boardPosY, uint shipSize, ShipDirection shipDirection) {
            for (int i = 0; i < shipSize; i++) {
                if (shipDirection == ShipDirection.Right) {
                    if (Board[boardPosX + i][boardPosY] != null) return false;
                } else {
                    if (Board[boardPosX][boardPosY + i] != null) return false;
                }
            }
            
            return true;
        }
    }
}