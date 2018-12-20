using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Domain;

namespace SaveSystem {
    public static class DalConverter {
        public static DAL.Game ConvertGame() {
            var dalGame = new DAL.Game {
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Winner = GameSystem.ActiveGame.Winner?.Name,
                RoundCounter = GameSystem.ActiveGame.RoundCounter,
                Moves = null,
                Players = null,
                Rules = null
            };

            // Because we need to point to the game
            dalGame.Rules = ConvertRules(dalGame);
            dalGame.Players = ConvertPlayers(dalGame);
            dalGame.Moves = ConvertMoves(dalGame, dalGame.Players, GameSystem.ActiveGame.Moves);

            return dalGame;
        }

        private static List<DAL.Player> ConvertPlayers(DAL.Game game) {
            var dalPlayers = new List<DAL.Player>();

            // Convert all Domain players to DAL player objects
            foreach (var domainPlayer in GameSystem.ActiveGame.Players) {
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
            HashSet<Pos> domainMoves) {
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

        private static List<DAL.Ship> ConvertShips(DAL.Player player, List<Ship> domainShips) {
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
            ShipStatus[] domainShipStatuses) {
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

        private static List<DAL.Move> ConvertMoves(DAL.Game game, List<DAL.Player> dalPlayers, List<Move> domainMoves) {
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
        
        private static HashSet<DAL.Rule> ConvertRules(DAL.Game game) {
            var dalRules = new HashSet<DAL.Rule>();

            foreach (var domainRule in GameSystem.ActiveGame.RuleSet) {
                var dalRule = new DAL.Rule {
                    Game = game,
                    RuleType = (int) domainRule.RuleType,
                    Value = domainRule.Value
                };

                dalRules.Add(dalRule);
            }

            return dalRules;
        }
    }
}