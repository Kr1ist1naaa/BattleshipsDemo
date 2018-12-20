using System.Collections.Generic;
using GameSystem.Logic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Player = Domain.Player;

namespace WebProgram {
    public class Program {
        public static void Main(string[] args) {
            GameSystem.ActiveGame.Players = new List<Player> {
                new Player("player1", ShipLogic.GenDefaultShipList()),
                new Player("dan", ShipLogic.GenDefaultShipList())
            };
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}