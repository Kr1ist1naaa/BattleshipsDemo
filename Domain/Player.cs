using System;
using System.Collections.Generic;
using Domain.Board;
using Initializers;

namespace Domain {
    public class Player {
        private string Name { get; }
        private ShipBoard ShipBoard { get; }
        private MoveBoard MoveBoard { get; }
        private List<Ship.Ship> Ships { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardSizeX"></param>
        /// <param name="boardSizeY"></param>
        /// <param name="playerName"></param>
        public Player(uint boardSizeX, uint boardSizeY, string playerName) {
            if (string.IsNullOrEmpty(playerName)) {
                throw new ArgumentException("Invalid player name");
            }
            
            ShipBoard = new ShipBoard(boardSizeX, boardSizeY);
            MoveBoard = new MoveBoard(boardSizeX, boardSizeY);
            Ships = ShipInitializer.GenDefaultShipSet();
            Name = playerName;
        }
    }
}