using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class AppDbContext : DbContext {
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseMySQL("server=localhost;database=test;user=root;password=root");
        }
    }
}