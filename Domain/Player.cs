using System;
using System.Collections.Generic;
using Domain.Board;
using MenuSystem;

namespace Domain {
    public class Player {
        private readonly Menu _menu;
        public readonly string Name;

        private readonly GameBoard _board;
        private readonly List<Ship.Ship> _ships;
        public Player TargetPlayer = null; // Reference to the player this player will attack
        public bool IsAlive = true;

        public Player(Menu menu, int boardSizeX, int boardSizeY, string playerName) {
            if (boardSizeX < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeX));
            }
            
            if (boardSizeY < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeY));
            }

            if (string.IsNullOrEmpty(playerName)) {
                throw new ArgumentOutOfRangeException(nameof(playerName));
            }

            Name = playerName;
            
            _menu = menu;
            _board = new GameBoard(_menu, boardSizeX, boardSizeY);
            _ships = Ship.Ship.GenDefaultShipSet();
            
            Console.WriteLine($"    - player {playerName} is now placing their ships");
            _board.PlaceShips(_ships);
        }

        public Move OutgoingAttack() {
            var firstLoop = true;
            int posX, posY;
            
            while (true) {
                // Ask player for attack location
                firstLoop = _menu.AskAttackCoords(firstLoop, out posX, out posY);
                
                // Check if attack coords are valid
                var isValidAttack = GameBoard.CheckValidAttackPos(TargetPlayer._board, posX, posY);
                if (!isValidAttack) continue;

                TargetPlayer.IncomingAttack(posX, posY);
                break;
            }
            
            // Create instance of Move with the details of the move
            return new Move(this, TargetPlayer, posX, posY);
        }

        private void IncomingAttack(int posX, int posY) {
            _board.IncomingAttack(posX, posY);
            IsAlive = _board.CheckIfAlive();
        }
    }
}