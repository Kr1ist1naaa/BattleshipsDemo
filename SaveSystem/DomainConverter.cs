using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Ship;

namespace SaveSystem {
    public static class DomainConverter {
        public static List<Player> ConvertPlayers(List<DAL.Player> dalPlayers) {
            var domainPlayers = new List<Player>();

            foreach (var dalPlayer in dalPlayers) {
                var domainPlayer = new Player {
                    Name = dalPlayer.Name,
                    MovesAgainstThisPlayer = ConvertPositions(dalPlayer.MovesAgainstThisPlayer),
                    Ships = ConvertShips(dalPlayer.Ships)
                };

                domainPlayers.Add(domainPlayer);
            }

            return domainPlayers;
        }

        private static HashSet<Pos> ConvertPositions(HashSet<DAL.Pos> dalPositions) {
            var domainPositions = new HashSet<Pos>();

            foreach (var dalPosition in dalPositions) {
                var domainPosition = new Pos {
                    X = dalPosition.X,
                    Y = dalPosition.Y
                };

                domainPositions.Add(domainPosition);
            }
            
            return domainPositions;
        }

        private static List<BaseShip> ConvertShips(List<DAL.Ship> dalShips) {
            var domainShips = new List<BaseShip>();

            foreach (var dalShip in dalShips) {
                var domainShip = new BaseShip {
                    Title = dalShip.Title,
                    Symbol = dalShip.Symbol,
                    Size = dalShip.Size,
                    Direction = (ShipDirection) dalShip.Direction,
                    ShipStatuses = ConvertShipStatus(dalShip.ShipStatuses),
                    ShipPos = new Pos {
                        X = dalShip.Position.X,
                        Y = dalShip.Position.Y
                    }
                };

                domainShips.Add(domainShip);
            }

            return domainShips;
        }

        private static ShipStatus[] ConvertShipStatus(int[] dalStatuses) {
            var domainStatuses = new ShipStatus[dalStatuses.Length];

            for (int i = 0; i < dalStatuses.Length; i++) {
                domainStatuses[i] = (ShipStatus) dalStatuses[i];
            }

            return domainStatuses;
        }

        public static List<Move> ConvertMoves(List<DAL.Move> dalMoves, List<Player> players) {
            var domainMoves = new List<Move>();

            // Convert all Domain players to DAL player objects
            foreach (var dalMove in dalMoves) {
                var domainMove = new Move {
                    FromPlayer = players.FirstOrDefault(m => m.Name.Equals(dalMove.FromPlayer)),
                    ToPlayer = players.FirstOrDefault(m => m.Name.Equals(dalMove.ToPlayer)),
                    AttackResult = (AttackResult) dalMove.AttackResult
                };

                domainMoves.Add(domainMove);
            }

            return domainMoves;
        }
    }
}