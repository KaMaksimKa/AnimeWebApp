using Microsoft.EntityFrameworkCore;

namespace AnimeWebApp.Models
{
    public class ApplicationContext:DbContext
    {
        public DbSet<Anime> Anime { get; set; } = null!;
        public DbSet<Genre> Genre { get; set; } = null!;
        public DbSet<Voiceover> Voiceover { get; set; } = null!;
        public ApplicationContext(DbContextOptions options) :base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*optionsBuilder.UseLazyLoadingProxies();*/
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=anime;Username=postgres;Password=ylikeMNjq7S8+");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anime>().HasIndex(a=>a.IdFromAnimeGo).IsUnique();
        }
    }
}
