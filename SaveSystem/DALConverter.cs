using System.Collections.Generic;
using DAL;
using Domain.Ship;

namespace SaveSystem {
    public static class DalConverter {
        public static List<Player> ConvertPlayers(List<Domain.Player> domainPlayers) {
            var dalPlayers = new List<Player>();

            // Convert all Domain players to DAL player objects
            foreach (var domainPlayer in domainPlayers) {
                var dalPlayer = new Player {
                    Name = domainPlayer.Name,
                    MovesAgainstThisPlayer = ConvertPositions(domainPlayer.MovesAgainstThisPlayer),
                    Ships = ConvertShips(domainPlayer.Ships)
                };

                dalPlayers.Add(dalPlayer);
            }


            return dalPlayers;
        }

        private static HashSet<Pos> ConvertPositions(HashSet<Domain.Pos> domainMoves) {
            var dalPositions = new HashSet<Pos>();

            // Convert all Domain positions to DAL position objects
            foreach (var domainPos in domainMoves) {
                var dalPos = new Pos {
                    X = domainPos.X,
                    Y = domainPos.Y
                };

                dalPositions.Add(dalPos);
            }

            return dalPositions;
        }

        private static List<Ship> ConvertShips(List<BaseShip> domainShips) {
            var dalShips = new List<Ship>();

            // Convert all Domain ships to DAL ship objects
            foreach (var domainShip in domainShips) {
                var dalShip = new Ship {
                    Title = domainShip.Title,
                    Symbol = domainShip.Symbol,
                    Size = domainShip.Size,
                    Direction = (int) domainShip.Direction,
                    ShipStatuses = ConvertShipStatus(domainShip.ShipStatuses),
                    Position = new Pos {
                        X = domainShip.ShipPos.X,
                        Y = domainShip.ShipPos.Y
                    }
                };

                dalShips.Add(dalShip);
            }

            return dalShips;
        }

        private static int[] ConvertShipStatus(ShipStatus[] domainShipStatuses) {
            var dalStatuses = new int[domainShipStatuses.Length];

            // Convert all Domain Ship status blocks to DAL ship status blocks
            for (int i = 0; i < domainShipStatuses.Length; i++) {
                dalStatuses[i] = (int) domainShipStatuses[i];
            }

            return dalStatuses;
        }

        public static List<Move> ConvertMoves(List<Domain.Move> domainMoves) {
            var dalMoves = new List<Move>();

            // Convert all Domain players to DAL player objects
            foreach (var domainMove in domainMoves) {
                var dalMove = new Move {
                    FromPlayer = domainMove.FromPlayer.Name,
                    ToPlayer = domainMove.ToPlayer.Name,
                    AttackResult = (int) domainMove.AttackResult
                };

                dalMoves.Add(dalMove);
            }

            return dalMoves;
        }
    }
}