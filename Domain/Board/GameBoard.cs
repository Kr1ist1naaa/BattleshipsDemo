using System;
using System.Collections.Generic;
using Domain.Ship;
using MenuSystem;

namespace Domain.Board {
    public class GameBoard {
        private readonly Menu _menu;
        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly BoardSlot[][] _board;

        public GameBoard(Menu menu, int boardSizeX, int boardSizeY) {
            CheckInitParams(boardSizeX, boardSizeY);

            _menu = menu;
            _sizeX = boardSizeX;
            _sizeY = boardSizeY;

            _board = new BoardSlot[boardSizeX][];
            for (var i = 0; i < boardSizeX; i++) {
                _board[i] = new BoardSlot[boardSizeY];
            }
        }
        
        public bool CheckIfAlive() {
            var shipBlockCount = 0;
            var hitShipBlockCount = 0;
            
            // Count total number of ship blocks and number of hit ship blocks
            for (int i = 0; i < _sizeX; i++) {
                for (int j = 0; j < _sizeY; j++) {
                    var boardSlot = _board[i][j];

                    if (boardSlot.State == BoardSlotState.Hit) {
                        hitShipBlockCount++;
                    }

                    if (boardSlot.Ship != null) {
                        shipBlockCount++;
                    }
                }
            }
            
            // If all blocks are hit
            return shipBlockCount != hitShipBlockCount;
        }

        public void IncomingAttack(int posX, int posY) {
            // posX and posY have been verified to be inside the board dimensions and have not been attacked before
            
            var boardSlot = _board[posX][posY];
            
            // Set the state of the board slot
            boardSlot.State = boardSlot.Ship == null ? BoardSlotState.Miss : BoardSlotState.Hit;
        }
        
        public static bool CheckValidAttackPos(GameBoard board, int posX, int posY) {
            if (posX < 0 || posX > board._sizeX) {
                return false;
            }
            
            if (posY < 0 || posY > board._sizeY) {
                return false;
            }

            return board._board[posX][posY].State == BoardSlotState.None;
        }

        private static void CheckInitParams(int boardSizeX, int boardSizeY) {
            if (boardSizeX < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeX));
            }
            
            if (boardSizeY < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeY));
            }
        }
        
        public void PlaceShips(IEnumerable<Ship.Ship> shipList) {
            foreach (var ship in shipList) {
                var firstLoop = true;

                while (true) {
                    // Ask player for placement location
                    _menu.AskShipPlacementPosition(out firstLoop, out var posX, out var posY, out var direction);

                    // Check if player has already attacked there
                    var isValidPos = CheckPlacementSpot(posX, posY, ship.Size, direction);
                    if (!isValidPos) continue;

                    // Place the ship
                    for (int i = 0; i < ship.Size; i++) {
                        var newPosX = posX + direction == ShipDirection.Right ? i : 0;
                        var newPosY = posY + direction == ShipDirection.Right ? 0 : i;

                        _board[newPosX][newPosY].Ship = ship;
                    }

                    break;
                }
            }
        }

        private bool CheckPlacementSpot(int posX, int posY, int shipSize, ShipDirection direction) {
            if (posX < 0 || posX > _sizeX) {
                return false;
            }

            if (posY < 0 || posY > _sizeY) {
                return false;
            }

            for (int i = 0; i < shipSize; i++) {
                var newPosX = posX + direction == ShipDirection.Right ? i : 0;
                var newPosY = posY + direction == ShipDirection.Right ? 0 : i;

                if (_board[newPosX][newPosY].Ship != null) {
                    return false;
                }
            }

            return true;
        }
    }
}