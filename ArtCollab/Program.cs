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


//Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.AccessDeniedPath = "/AccessDenied";
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

app.MapRazorPages();

app.Run();
