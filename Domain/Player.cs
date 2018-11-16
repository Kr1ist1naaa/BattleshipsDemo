using System;
using System.Collections.Generic;
using Domain.Board;
using Initializers;
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
            CheckParams(boardSizeX, boardSizeY, playerName);

            Name = playerName;
            
            _menu = menu;
            _board = new GameBoard(_menu, boardSizeX, boardSizeY);
            _ships = ShipInitializer.GenDefaultShipSet();
            
            _board.PlaceShips(_ships);
        }

        public void OutgoingAttack() {
            var firstLoop = true;
            
            while (true) {
                // Ask player for attack location
                _menu.AskAttackCoords(out firstLoop, out var posX, out var posY);
                
                // Check if attack coords are valid
                var isValidAttack = GameBoard.CheckValidAttackPos(TargetPlayer._board, posX, posY);
                if (!isValidAttack) continue;

                TargetPlayer.IncomingAttack(posX, posY);
                break;
            }
        }

        private void IncomingAttack(int posX, int posY) {
            _board.IncomingAttack(posX, posY);
            IsAlive = _board.CheckIfAlive();
        }

        private static void CheckParams(int boardSizeX, int boardSizeY, string playerName) {
            if (boardSizeX < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeX));
            }
            
            if (boardSizeY < 2) {
                throw new ArgumentOutOfRangeException(nameof(boardSizeY));
            }

            if (string.IsNullOrEmpty(playerName)) {
                throw new ArgumentOutOfRangeException(nameof(playerName));
            }
        }

    }
}