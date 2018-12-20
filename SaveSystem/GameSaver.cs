using System.Linq;
using DAL;
using Domain;

namespace SaveSystem {
    public static class GameSaver {
        public static void Save() {
            using (var ctx = new AppDbContext()) {
                var dalGame = DalConverter.ConvertGame();
                ctx.Games.Add(dalGame);
                ctx.SaveChanges();

                GameSystem.ActiveGame.GameId = ctx.Games.Last().Id;
            }
        }

        public static void Load(int gameId) {
            using (var ctx = new AppDbContext()) {
                var dalGame = ctx.Games.Find(gameId);

                // Build game-dependant objects
                var domainPlayers = DomainConverter.GetAndConvertPlayers(ctx, gameId);
                var domainMoves = DomainConverter.GetAndConvertMoves(ctx, gameId, domainPlayers);

                // Load game data into static context
                GameSystem.ActiveGame.Winner =
                    domainPlayers.FirstOrDefault(player => player.Name.Equals(dalGame.Winner));
                GameSystem.ActiveGame.TurnCount = dalGame.TurnCount;
                GameSystem.ActiveGame.Moves.AddRange(domainMoves);
                GameSystem.ActiveGame.Players = domainPlayers;
                GameSystem.ActiveGame.GameId = dalGame.Id;

                // Load rule values into static context
                var dalRules = ctx.Rules.Where(rule => rule.Game.Id == gameId);
                foreach (var dalRule in dalRules) {
                    GameSystem.ActiveGame.ChangeRule((RuleType) dalRule.RuleType, dalRule.Value);
                }
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

        public static void OverwriteSave() {
            if (GameSystem.ActiveGame.GameId == null) {
                return;
            }

            Delete((int) GameSystem.ActiveGame.GameId);
            Save();
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