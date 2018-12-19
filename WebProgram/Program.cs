using System.Collections.Generic;
using Domain;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebProgram {
    public class Program {
        public static void Main(string[] args) {
            GameSystem.GameLogic.Players = new List<Player> {
                new Player("player1"),
                new Player("dan")
            };
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}