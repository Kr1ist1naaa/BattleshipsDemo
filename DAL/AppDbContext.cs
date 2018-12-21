using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class AppDbContext : DbContext {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<MovesAgainstPlayer> MovesAgainstPlayers { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<ShipStatus> ShipStatuses { get; set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseMySql("server=localhost;database=test;user=root;password=root");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Define some db field restrictions
            modelBuilder.Entity<Player>()
                .Property(p => p.Name)
                .HasMaxLength(32);
            modelBuilder.Entity<Ship>()
                .Property(p => p.Title)
                .HasMaxLength(32);
            modelBuilder.Entity<Ship>()
                .Property(p => p.Symbol)
                .HasMaxLength(1);
            modelBuilder.Entity<Ship>()
                .Property(p => p.Direction)
                .HasMaxLength(1);
        }
    }
}