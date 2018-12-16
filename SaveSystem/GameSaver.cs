using System.Linq;
using DAL;
using Domain;
using Domain.DomainRule;
using Game = Domain.Game;

namespace SaveSystem {
    public static class GameSaver {
        public static void Save(Game domainGame) {
            using (var ctx = new AppDbContext()) {
                var dalGame = DalConverter.ConvertGame(domainGame);
                ctx.Games.Add(dalGame);
                ctx.SaveChanges();

                domainGame.GameId = ctx.Games.Last().Id;
            }
        }

        public static Game Load(int gameId) {
            using (var ctx = new AppDbContext()) {
                var dalGame = ctx.Games.Find(gameId);

                // Build game-dependant objects
                var domainPlayers = DomainConverter.GetAndConvertPlayers(ctx, gameId);
                var domainMoves = DomainConverter.GetAndConvertMoves(ctx, gameId, domainPlayers);

                // Build game
                var domainGame = new Game {
                    Winner = domainPlayers.FirstOrDefault(player => player.Name.Equals(dalGame.Winner)),
                    TurnCount = dalGame.TurnCount,
                    Moves = domainMoves,
                    Players = domainPlayers,
                    GameId = dalGame.Id
                };

                // Load rules into static context
                Rules.ResetAllToDefault();
                var dalRules = ctx.Rules.Where(rule => rule.Game.Id == gameId);
                foreach (var dalRule in dalRules) {
                    Rules.ChangeRule((RuleType) dalRule.RuleType, dalRule.Value);
                }

                return domainGame;
            }
        }

        public static void Delete(int gameId) {
            using (var ctx = new AppDbContext()) {
                var games = ctx.Games.Where(game => game.Id == gameId);
                var moves = ctx.Moves.Where(move => move.Game.Id == gameId);
                var rules = ctx.Rules.Where(rule => rule.Game.Id == gameId);
                var players = ctx.Players.Where(player => player.Game.Id == gameId);
                
                foreach (var player in players) {
                    var ships = ctx.Ships.Where(ship => ship.Player.Id == player.Id);
                    
                    foreach (var ship in ships) {
                        var statuses = ctx.ShipStatuses.Where(status => status.Ship.Id == ship.Id);
                        ctx.ShipStatuses.RemoveRange(statuses);
                    }
                    
                    ctx.Ships.RemoveRange(ships);
                }
                
                foreach (var player in players) {
                    var playerMoves = ctx.MovesAgainstPlayers.Where(move => move.Player.Id == player.Id);
                    ctx.MovesAgainstPlayers.RemoveRange(playerMoves);
                }
                
                ctx.Rules.RemoveRange(rules);
                ctx.Moves.RemoveRange(moves);
                ctx.Players.RemoveRange(players);
                ctx.Games.RemoveRange(games);
                
                ctx.SaveChanges();
            }
        }

        public static void OverwriteSave(Game domainGame) {
            if (domainGame.GameId == null) {
                return;
            }

            Delete((int) domainGame.GameId);
            Save(domainGame);
        }

        public static string[][] GetSaveGameList() {
            using (var ctx = new AppDbContext()) {
                var saveGames = ctx.Games.Select(
                    m => new[] {
                        m.Id.ToString(),
                        m.TurnCount.ToString(),
                        m.Date
                    }
                );

                return saveGames.ToArray();
            }
        }
    }
}