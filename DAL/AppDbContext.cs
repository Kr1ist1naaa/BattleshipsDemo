using System.Data.Entity;

namespace DAL {
    public class AppDbContext : DbContext {
        public DbSet<Game> Games { get; set; }




    }
}