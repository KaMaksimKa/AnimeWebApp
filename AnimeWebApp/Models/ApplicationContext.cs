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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anime>().HasIndex(a=>a.IdFromAnimeGo).IsUnique();
        }
    }
}
