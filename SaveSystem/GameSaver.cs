using System.Linq;
using DAL;
using Domain;
using GameSystem;

namespace SaveSystem {
    public static class GameSaver {
        public static void Save() {
            if (ActiveGame.GameId != null) {
                Delete((int) ActiveGame.GameId);
            }
            
            using (var ctx = new AppDbContext()) {
                var dalGame = DalConverter.ConvertGame();
                ctx.Games.Add(dalGame);
                ctx.SaveChanges();

                ActiveGame.GameId = ctx.Games.Last().Id;
            }
        }

        public static void Load(int gameId) {
            using (var ctx = new AppDbContext()) {
                // Reset current game to initial state
                ActiveGame.Init();
                
                // Find the DAL game
                var dalGame = ctx.Games.Find(gameId);

                // Load game data into static context
                ActiveGame.Players = DomainConverter.GetAndConvertPlayers(ctx, gameId);
                ActiveGame.Moves = DomainConverter.GetAndConvertMoves(ctx, gameId, ActiveGame.Players);
                ActiveGame.Winner = ActiveGame.Players.FirstOrDefault(player => player.Name.Equals(dalGame.Winner));
                ActiveGame.RoundCounter = dalGame.RoundCounter;
                ActiveGame.GameId = dalGame.Id;

                // Load rules into static context
                var dalRules = ctx.Rules.Where(rule => rule.Game.Id == gameId);
                foreach (var dalRule in dalRules) {
                    ActiveGame.TryChangeRule((RuleType) dalRule.RuleType, dalRule.Value);
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

        public static string[][] GetSaveGameList() {
            using (var ctx = new AppDbContext()) {
                var saveGames = ctx.Games.Select(
                    m => new[] {
                        m.Id.ToString(),
                        m.RoundCounter.ToString(),
                        m.Date
                    }
                );

                return saveGames.ToArray();
            }
        }
    }
}