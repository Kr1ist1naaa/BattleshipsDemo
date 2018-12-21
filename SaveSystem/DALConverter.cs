using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SaveSystem {
    public static class DalConverter {
        public static DAL.Game ConvertGame(Domain.Game domainGame) {
            var dalGame = new DAL.Game {
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Winner = domainGame.Winner?.Name,
                TurnCount = domainGame.TurnCount,
                Moves = null,
                Players = null
            };

            dalGame.Players = ConvertPlayers(dalGame, domainGame.Players);
            dalGame.Moves = ConvertMoves(dalGame, dalGame.Players, domainGame.Moves);

            return dalGame;
        }

        private static List<DAL.Player> ConvertPlayers(DAL.Game game, List<Domain.Player> domainPlayers) {
            var dalPlayers = new List<DAL.Player>();

            // Convert all Domain players to DAL player objects
            foreach (var domainPlayer in domainPlayers) {
                var dalPlayer = new DAL.Player {
                    Name = domainPlayer.Name,
                    MovesAgainstThisPlayer = null,
                    Ships = null,
                    Game = game
                };

                dalPlayer.MovesAgainstThisPlayer =
                    ConvertMovesAgainstPlayer(dalPlayer, domainPlayer.MovesAgainstThisPlayer);
                dalPlayer.Ships = ConvertShips(dalPlayer, domainPlayer.Ships);

                dalPlayers.Add(dalPlayer);
            }


            return dalPlayers;
        }

        private static HashSet<DAL.MovesAgainstPlayer> ConvertMovesAgainstPlayer(DAL.Player player,
            HashSet<Domain.Pos> domainMoves) {
            var dalMoves = new HashSet<DAL.MovesAgainstPlayer>();

            // Convert all Domain positions to DAL position objects
            foreach (var domainMove in domainMoves) {
                var dalMove = new DAL.MovesAgainstPlayer {
                    Player = player,
                    X = domainMove.X,
                    Y = domainMove.Y
                };

                dalMoves.Add(dalMove);
            }

            return dalMoves;
        }

        private static List<DAL.Ship> ConvertShips(DAL.Player player, List<Domain.Ship.Ship> domainShips) {
            var dalShips = new List<DAL.Ship>();

            // Convert all Domain ships to DAL ship objects
            foreach (var domainShip in domainShips) {
                var dalShip = new DAL.Ship {
                    Player = player,
                    Title = domainShip.Title,
                    Symbol = domainShip.Symbol,
                    Size = domainShip.Size,
                    Direction = (int) domainShip.Direction,
                    ShipStatuses = null,
                    X = domainShip.ShipPos.X,
                    Y = domainShip.ShipPos.Y
                };

                dalShip.ShipStatuses = ConvertShipStatus(dalShip, domainShip.ShipStatuses);

                dalShips.Add(dalShip);
            }

            return dalShips;
        }

        private static List<DAL.ShipStatus> ConvertShipStatus(DAL.Ship dalShip,
            Domain.Ship.ShipStatus[] domainShipStatuses) {
            var dalStatuses = new List<DAL.ShipStatus>();

            // Convert all Domain Ship status blocks to DAL ship status blocks
            for (int i = 0; i < domainShipStatuses.Length; i++) {
                var dalStatus = new DAL.ShipStatus {
                    Ship = dalShip,
                    Status = (int) domainShipStatuses[i],
                    Offset = i
                };

                dalStatuses.Add(dalStatus);
            }

            return dalStatuses;
        }

        private static List<DAL.Move> ConvertMoves(DAL.Game game, List<DAL.Player> dalPlayers,
            List<Domain.Move> domainMoves) {
            var dalMoves = new List<DAL.Move>();

            // Convert all Domain players to DAL player objects
            foreach (var domainMove in domainMoves) {
                var dalMove = new DAL.Move {
                    Game = game,
                    FromPlayer = dalPlayers.FirstOrDefault(player => player.Name.Equals(domainMove.FromPlayer.Name)),
                    ToPlayer = dalPlayers.FirstOrDefault(player => player.Name.Equals(domainMove.ToPlayer.Name)),
                    MoveResult = (int) domainMove.AttackResult,
                    X = domainMove.Pos.X,
                    Y = domainMove.Pos.Y
                };

                dalMoves.Add(dalMove);
            }

            return dalMoves;
        }
    }
}