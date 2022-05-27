using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimeWebApp.Models
{
    public class ApplicationContext: IdentityDbContext<AppUser>
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
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Anime>().HasIndex(a => a.IdFromAnimeGo).IsUnique();
            modelBuilder.Entity<Genre>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<Dubbing>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<MpaaRate>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<Status>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<Studio>().HasIndex(g => g.Title).IsUnique();
            modelBuilder.Entity<TypeAnime>().HasIndex(g => g.Title).IsUnique();
        }
        public static async Task CreateAdminAccount(IServiceProvider serviceprovider, IConfiguration configuration)
        {
            await using var scope = serviceprovider.CreateAsyncScope();
            UserManager<AppUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];
            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                AppUser user = new AppUser
                {
                    UserName = username,
                    Email = email
                };
                IdentityResult result = await userManager
                .CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
