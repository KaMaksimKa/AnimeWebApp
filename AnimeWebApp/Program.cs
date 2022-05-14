using System.Diagnostics;
using System.Text.Json.Serialization;
using AnimeWebApp.Infrastructure;
using AnimeWebApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(options =>options.EnableEndpointRouting=false ).AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddDbContext<ApplicationContext>(/*optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}*/);
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddTransient<IAnimeRepository, EfAnimeRepository>();
var app = builder.Build();

app.UseStatusCodePages();
app.UseDeveloperExceptionPage();
app.UseStaticFiles(); 

 app.UseMvc(routeBuilder =>
{
    routeBuilder.Routes.Add(new FilterAnimeRoute(routeBuilder.DefaultHandler!)
    {
        SortAndNumberPage = new SortAndNumberPage{DefaultSort = "date-add-desc"},
        Controller = "Anime",
        Action = "Filter"
    });
    routeBuilder.Routes.Add(new SearchAnimeRoute(routeBuilder.DefaultHandler!)
    {
        SortAndNumberPage = new SortAndNumberPage { DefaultSort = "date-add-desc" },
        Controller = "Anime",
        Action = "Search",
    });
    routeBuilder.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}"
        );
    
} );

app.Run();
