using Microsoft.EntityFrameworkCore;

namespace AnimeWebApp.Models
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Anime> Animes { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Dubbing> Dubbing { get; set; } = null!;
        public DbSet<MpaaRate> MpaaRates { get; set; } = null!;
        public DbSet<Studio> Studios { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;
        public DbSet<TypeAnime> Types { get; set; } = null!;
        public ApplicationContext(DbContextOptions options) :base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*optionsBuilder.UseLazyLoadingProxies();*/
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=animego;Username=postgres;Password=ylikeMNjq7S8+");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anime>().HasIndex(a => a.IdFromAnimeGo).IsUnique();
            modelBuilder.Entity<Genre>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<Dubbing>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<MpaaRate>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<Status>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<Studio>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<TypeAnime>().HasIndex(g => g.Title).IsUnique();
        }
    }
}
