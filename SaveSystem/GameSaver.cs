using System.Linq;
using DAL;
using Domain;
using Game = Domain.Game;

namespace SaveSystem {
    public static class GameSaver {
        private static readonly AppDbContext Ctx = new AppDbContext();

        public static void Save(Game domainGame) {
            var dalGame = DalConverter.ConvertGame(domainGame);
            Ctx.Games.Add(dalGame);
            Ctx.SaveChanges();

            domainGame.GameId = Ctx.Games.Last().Id;
        }

        public static Game Load(int gameId) {
            var dalGame = Ctx.Games.Find(gameId);

            var domainPlayers = DomainConverter.GetAndConvertPlayers(Ctx, gameId);
            var domainMoves = DomainConverter.GetAndConvertMoves(Ctx, gameId, domainPlayers);

            var domainGame = new Game {
                Winner = domainPlayers.FirstOrDefault(player => player.Name.Equals(dalGame.Winner)),
                TurnCount = dalGame.TurnCount,
                Moves = domainMoves,
                Players = domainPlayers,
                GameId = dalGame.Id
            };

            return domainGame;
        }

        public static void Delete(int gameId) {
            var save = Ctx.Games.Find(gameId);
            
            Ctx.Games.Remove(save);
            Ctx.SaveChanges();
        }

        public static void OverwriteSave(Game domainGame) {
            if (domainGame.GameId == null) {
                return;
            }

            Delete((int) domainGame.GameId);
            Save(domainGame);
        }

        public static string[][] GetSaveGameList() {
            var saveGames = Ctx.Games.Select(
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