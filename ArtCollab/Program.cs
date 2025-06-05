using System.Text;
using ArtCollab;
using ArtCollab.Services;
using Data;
using Logic.Interfaces;
using Logic.Managers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;



//Managers
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<ArtworkManager>();
builder.Services.AddScoped<EventManager>();
builder.Services.AddScoped<CollectionManager>();
builder.Services.AddScoped<CommentManager>();

//Repositories
builder.Services.AddScoped<IUserRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new UserRepository(connectionString);
});

builder.Services.AddScoped<IArtworkRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new ArtworkRepository(connectionString);
});

builder.Services.AddScoped<IEventRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new EventRepository(connectionString);
});
builder.Services.AddScoped<ICollectionRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new CollectionRepository(connectionString);
});
builder.Services.AddScoped<ICommentRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("DefaultConnection");
    return new CommentRepository(connectionString);
});

//NewAdmin

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/NewAdmin", "AdminOnly");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/AccessDenied"; // Maak een Razor page /AccessDenied.cshtml aan
    options.LoginPath = "/Login"; // Voor niet-ingelogde gebruikers
});


//Cookies

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(14); // maxduur
        options.SlidingExpiration = true; // verlengt cookie bij elke activiteit
    });



var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // moet vóór UseAuthorization
app.UseAuthorization();

app.MapGet("/", context => {
    context.Response.Redirect("/Home");
    return Task.CompletedTask;
});


app.Run();
