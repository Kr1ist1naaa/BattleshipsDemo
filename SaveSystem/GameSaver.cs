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

        public static bool DeleteSaveGame(int index) {
            DAL.Game save;

            // Find game by index, assign to var and check if it returned null
            if ((save = Ctx.Games.Find(index)) == null) {
                return false;
            }

            // Check if game entry was successfully removed
            if (Ctx.Games.Remove(save) != null) {
                return false;
            }

            Ctx.SaveChanges();

            return true;
        }

        public static void OverwriteSave(Game domainGame) {
            if (domainGame.GameId == null) {
                return;
            }

            DeleteSaveGame((int) domainGame.GameId);
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