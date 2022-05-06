using System.Diagnostics;
using AnimeWebApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(options =>options.EnableEndpointRouting=false );
builder.Services.AddDbContext<ApplicationContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddTransient<IAnimeRepository, EfAnimeRepository>();
var app = builder.Build();

app.UseStatusCodePages();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseMvc(routeBuilder =>
{
    routeBuilder.MapRoute(
        name: null,
        template: "anime/page{page:int}",
        defaults: new { controller = "Anime",action = "Index" }
    );
    routeBuilder.MapRoute(
        name:null,
        template:"anime/{action}/page{page:int}",
        defaults: new { controller = "Anime"}
        );
    routeBuilder.MapRoute(
        name:"default",
        template:"{controller=Anime}/{action=Index}/{id?}"
        );
} );

app.Run();
