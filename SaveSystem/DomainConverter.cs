using System.Collections.Generic;
using System.Linq;
using Domain;

namespace SaveSystem {
    public static class DomainConverter {
        public static List<Player> GetAndConvertPlayers(DAL.AppDbContext ctx, int gameId) {
            var dalPlayers = ctx.Players.Where(player => player.Game.Id == gameId);

            var domainPlayers = new List<Player>();
            foreach (var dalPlayer in dalPlayers) {
                var domainPlayer = new Player {
                    Name = dalPlayer.Name,
                    MovesAgainstThisPlayer = GetAndConvertMovesAgainstPlayer(ctx, dalPlayer.Id),
                    Ships = GetAndConvertShips(ctx, dalPlayer.Id)
                };

                domainPlayers.Add(domainPlayer);
            }

            return domainPlayers;
        }

        private static HashSet<Pos> GetAndConvertMovesAgainstPlayer(DAL.AppDbContext ctx, int playerId) {
            var dalMoves = ctx.MovesAgainstPlayers.Where(move => move.Player.Id == playerId);

            var domainMoves = new HashSet<Pos>();
            foreach (var dalMove in dalMoves) {
                var domainMove = new Pos {
                    X = dalMove.X,
                    Y = dalMove.Y
                };

                domainMoves.Add(domainMove);
            }

            return domainMoves;
        }

        private static List<Ship> GetAndConvertShips(DAL.AppDbContext ctx, int playerId) {
            var dalShips = ctx.Ships.Where(ship => ship.Player.Id == playerId);

            var domainShips = new List<Ship>();
            foreach (var dalShip in dalShips) {
                var domainShip = new Ship {
                    Title = dalShip.Title,
                    Symbol = dalShip.Symbol,
                    Size = dalShip.Size,
                    Direction = (ShipDirection) dalShip.Direction,
                    ShipStatuses = GetAndConvertShipStatus(ctx, dalShip.Id),
                    ShipPos = new Pos {
                        X = dalShip.X,
                        Y = dalShip.Y
                    },
                    IsPlaced = true
                };

                domainShips.Add(domainShip);
            }

            return domainShips;
        }

        private static ShipStatus[] GetAndConvertShipStatus(DAL.AppDbContext ctx, int shipId) {
            var dalStatuses = ctx.ShipStatuses.Where(status => status.Ship.Id == shipId);

            var domainStatuses = new ShipStatus[dalStatuses.Count()];
            foreach (var dalStatus in dalStatuses) {
                domainStatuses[dalStatus.Offset] = (ShipStatus) dalStatus.Status;
            }

            return domainStatuses;
        }

        public static List<Move> GetAndConvertMoves(DAL.AppDbContext ctx, int gameId,
            List<Player> players) {
            var dalMoves = ctx.Moves.Where(move => move.Game.Id == gameId);

            var domainMoves = new List<Move>();
            foreach (var dalMove in dalMoves) {
                var domainMove = new Move {
                    FromPlayer = players.FirstOrDefault(player => player.Name.Equals(dalMove.FromPlayer.Name)),
                    ToPlayer = players.FirstOrDefault(player => player.Name.Equals(dalMove.ToPlayer.Name)),
                    AttackResult = (AttackResult) dalMove.MoveResult,
                    Pos = new Pos {
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