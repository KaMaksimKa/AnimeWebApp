using System.Diagnostics;
using AnimeWebApp.Infrastructure;
using AnimeWebApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc(options =>options.EnableEndpointRouting=false );
builder.Services.AddSingleton<PagingAnimeHandlersFactory>();
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
    routeBuilder.Routes.Add(new AnimesRoute(routeBuilder.DefaultHandler!){DefaultSort = "date-add-desc"});
    routeBuilder.MapRoute(
        name: "default",
        template: "{controller}/{action=Index}/{id?}"
        );
    
} );

app.Run();
