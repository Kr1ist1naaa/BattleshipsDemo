using System.Collections.Generic;
using System.Linq;

namespace SaveSystem {
    public static class DomainConverter {
        public static List<Domain.Player> GetAndConvertPlayers(DAL.AppDbContext ctx, int gameId) {
            var dalPlayers = ctx.Players.Where(player => player.Game.Id == gameId);

            var domainPlayers = new List<Domain.Player>();
            foreach (var dalPlayer in dalPlayers) {
                var domainPlayer = new Domain.Player {
                    Name = dalPlayer.Name,
                    MovesAgainstThisPlayer = GetAndConvertMovesAgainstPlayer(ctx, dalPlayer.Id),
                    Ships = GetAndConvertShips(ctx, dalPlayer.Id)
                };

                domainPlayers.Add(domainPlayer);
            }

            return domainPlayers;
        }

        private static HashSet<Domain.Pos> GetAndConvertMovesAgainstPlayer(DAL.AppDbContext ctx, int playerId) {
            var dalMoves = ctx.MovesAgainstPlayers.Where(move => move.Player.Id == playerId);

            var domainMoves = new HashSet<Domain.Pos>();
            foreach (var dalMove in dalMoves) {
                var domainMove = new Domain.Pos {
                    X = dalMove.X,
                    Y = dalMove.Y
                };

                domainMoves.Add(domainMove);
            }

            return domainMoves;
        }

        private static List<Domain.Ship.Ship> GetAndConvertShips(DAL.AppDbContext ctx, int playerId) {
            var dalShips = ctx.Ships.Where(ship => ship.Player.Id == playerId);

            var domainShips = new List<Domain.Ship.Ship>();
            foreach (var dalShip in dalShips) {
                var domainShip = new Domain.Ship.Ship {
                    Title = dalShip.Title,
                    Symbol = dalShip.Symbol,
                    Size = dalShip.Size,
                    Direction = (Domain.Ship.ShipDirection) dalShip.Direction,
                    ShipStatuses = GetAndConvertShipStatus(ctx, dalShip.Id),
                    ShipPos = new Domain.Pos {
                        X = dalShip.X,
                        Y = dalShip.Y
                    }
                };

                domainShips.Add(domainShip);
            }

            return domainShips;
        }

        private static Domain.Ship.ShipStatus[] GetAndConvertShipStatus(DAL.AppDbContext ctx, int shipId) {
            var dalStatuses = ctx.ShipStatuses.Where(status => status.Ship.Id == shipId);

            var domainStatuses = new Domain.Ship.ShipStatus[dalStatuses.Count()];
            foreach (var dalStatus in dalStatuses) {
                domainStatuses[dalStatus.Offset] = (Domain.Ship.ShipStatus) dalStatus.Status;
            }

            return domainStatuses;
        }

        public static List<Domain.Move> GetAndConvertMoves(DAL.AppDbContext ctx, int gameId,
            List<Domain.Player> players) {
            var dalMoves = ctx.Moves.Where(move => move.Game.Id == gameId);

            var domainMoves = new List<Domain.Move>();
            foreach (var dalMove in dalMoves) {
                var domainMove = new Domain.Move {
                    FromPlayer = players.FirstOrDefault(player => player.Name.Equals(dalMove.FromPlayer.Name)),
                    ToPlayer = players.FirstOrDefault(player => player.Name.Equals(dalMove.ToPlayer.Name)),
                    AttackResult = (Domain.AttackResult) dalMove.MoveResult,
                    Pos = new Domain.Pos {
                        X = dalMove.X,
                        Y = dalMove.Y
                    }
                };

                domainMoves.Add(domainMove);
            }

            return domainMoves;
        }
    }
}