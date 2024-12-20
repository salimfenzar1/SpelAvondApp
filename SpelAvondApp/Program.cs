using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Application;
using SpelAvondApp.Data;
using SpelAvondApp.Domain.Models;
using SpelAvondApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Check of de applicatie in development of productie draait
var isDevelopment = builder.Environment.IsDevelopment();

// Connection string voor de Identity database
var identityConnectionString = isDevelopment
    ? builder.Configuration.GetConnectionString("DefaultConnection") // Lokale development string
    : Environment.GetEnvironmentVariable("IDENTITY_CONNSTR"); // Connection string uit Azure environment variable

if (identityConnectionString == null)
    throw new InvalidOperationException("Connection string 'IDENTITY_CONNSTR' not found.");

// Connection string voor de Spellen database
var spellenDbConnectionString = isDevelopment
    ? builder.Configuration.GetConnectionString("SpellenDbConnection") // Lokale development string
    : Environment.GetEnvironmentVariable("SPELLEN_CONNSTR"); // Connection string uit Azure environment variable



if (spellenDbConnectionString == null)
    throw new InvalidOperationException("Connection string 'SPELLEN_CONNSTR' not found.");

// Configureer de Identity database met ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

// Configureer de spellen-database met SpellenDbContext
builder.Services.AddDbContext<SpellenDbContext>(options =>
    options.UseSqlServer(spellenDbConnectionString));


// Identity configureren zonder rollen
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IBordspelService, BordspelService>();
builder.Services.AddScoped<ISpellenRepository, SpellenRepository>();
builder.Services.AddScoped<IBordspellenAvondService, BordspellenAvondService>();
builder.Services.AddScoped<IInschrijvingService, InschrijvingService>();
 builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
