using System;
using System.Collections.Generic;
using Domain;

namespace GameSystem.Logic {
    public static class GameLogic {
        private static readonly Random Random = new Random();
        
        public static bool AutoPlaceShips(Player player) {
            const int tryAmount = 64;
            var boardSize = ActiveGame.GetRuleVal(RuleType.BoardSize);

            foreach (var ship in player.Ships) {
                var attemptCount = 0;

                // Attempt to place ship at X different locations
                while (attemptCount < tryAmount) {
                    var pos = new Pos(Random.Next(0, boardSize), Random.Next(0, boardSize));
                    var dir = Random.Next(0, 2) == 1 ? ShipDirection.Right : ShipDirection.Down;

                    if (!PlayerLogic.CheckValidShipPlacementPos(player, pos, ship.Size, dir)) {
                        attemptCount++;
                        continue;
                    }

                    ship.SetLocation(pos, dir);
                    break;
                }

                if (attemptCount >= tryAmount) {
                    return false;
                }
            }

            return true;
        }
        
        public static Player FindNextPlayer(IReadOnlyList<Player> players, Player player) {
            var playerCount = ActiveGame.GetRuleVal(RuleType.PlayerCount);
            Player nextPlayer = null;

            for (int i = 0; i < playerCount; i++) {
                // Find current player's index
                if (players[i] != player) {
                    continue;
                }

                for (int j = 1; j < playerCount; j++) {
                    var tmpPlayer = players[i + j - (i + j < playerCount ? 0 : playerCount)];

                    if (PlayerLogic.IsAlive(tmpPlayer)) {
                        nextPlayer = tmpPlayer;
                    }
                }
            }

            // Should not run
            if (nextPlayer == null) {
                throw new NullReferenceException(nameof(nextPlayer));
            }

            return nextPlayer;
        }
    }
}