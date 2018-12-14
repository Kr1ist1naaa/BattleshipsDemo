using System;
using System.Globalization;
using System.Linq;
using DAL;
using Domain;

namespace SaveSystem {
    public static class GameSaver {
        private static readonly AppDbContext Ctx = new AppDbContext();
        
        public static void Save(BaseGame domainGame) {
            // Convert Domain game to DAL game object
            var dalGame = new Game {
                Date = DateTime.Now.ToString(CultureInfo.CurrentCulture),
                Winner = domainGame.Winner.Name,
                TurnCount = domainGame.TurnCount,
                Moves = DalConverter.ConvertMoves(domainGame.Moves),
                Players = DalConverter.ConvertPlayers(domainGame.Players)
            };

            Ctx.Games.Add(dalGame);
            Ctx.SaveChanges();
        }
        
        public static BaseGame Load(int index) {
            var dalGame = Ctx.Games.Find(index);
            if (dalGame == null) {
                return null;
            }

            var domainPlayers = DomainConverter.ConvertPlayers(dalGame.Players);
            var domainWinner = domainPlayers.FirstOrDefault(m => m.Name.Equals(dalGame.Winner));
            var domainMoves = DomainConverter.ConvertMoves(dalGame.Moves, domainPlayers);
            
            var domainGame = new BaseGame {
                Winner = domainWinner,
                TurnCount = dalGame.TurnCount,
                Moves = domainMoves,
                Players = domainPlayers
            };
            
            return domainGame;
        }

        public static bool DeleteSaveGame(int index) {
            Game save;

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
        
        public static void OverwriteSave(int index, BaseGame domainGame) {
            DeleteSaveGame(index);
            Save(domainGame);
        }
        
        public static string[][] GetSaveGameList() {
            var saveGames = Ctx.Games.Select(
                m => new string[] {
                    m.GameId.ToString(), 
                    m.Date
                }
            );
            
            return saveGames.ToArray();
        }

    }
}