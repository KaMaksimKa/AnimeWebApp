using System.Diagnostics;
using System.Text.Json.Serialization;
using AnimeWebApp.Infrastructure;
using AnimeWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(options =>options.EnableEndpointRouting=false ).AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddDbContext<ApplicationContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddTransient<IAnimeRepository, EfAnimeRepository>();
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
    }).AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();
var app = builder.Build();

app.UseStatusCodePages();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseAuthentication();

 app.UseMvc(routeBuilder =>
{
    routeBuilder.Routes.Add(new FilterAnimeRoute(routeBuilder.DefaultHandler!)
    {
        SortAndNumberPage = new SortAndNumberPageForRoute{DefaultSort = "date-add-desc"},
        Controller = "Anime",
        Action = "Filter"
    });
    routeBuilder.Routes.Add(new SearchAnimeRoute(routeBuilder.DefaultHandler!)
    {
        SortAndNumberPage = new SortAndNumberPageForRoute { DefaultSort = "date-add-desc" },
        Controller = "Anime",
        Action = "Search",
    });
    routeBuilder.MapRoute(
        name:null,
        template: "manageranime/changefriendlyurl/{animeAttribute}",
        defaults: new { controller  = "ManagerAnime",action = "ChangeFriendlyUrl" }
        );
    routeBuilder.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}"
        );
    
});

ApplicationContext.CreateAdminAccount(app.Services, app.Configuration).Wait();

app.Run();
