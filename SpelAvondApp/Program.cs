using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Data;
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

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity configureren
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    await SeedRolesAndAdminUser(roleManager, userManager);
}

// Methode om rollen en een admin-gebruiker te seeden
async Task SeedRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    // Check of de Administrator rol bestaat, zo niet, maak hem aan
    if (!await roleManager.RoleExistsAsync("Administrator"))
    {
        await roleManager.CreateAsync(new IdentityRole("Administrator"));
    }

    // Voeg een admin-gebruiker toe als deze nog niet bestaat
    var adminUser = await userManager.FindByEmailAsync("admin@example.com");
    if (adminUser == null)
    {
        var user = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "AdminPassword123!"); 
        if (result.Succeeded)
        {
            // Ken de rol Administrator toe aan de gebruiker
            await userManager.AddToRoleAsync(user, "Administrator");
        }
    }
}

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Zorg ervoor dat authenticatie is ingeschakeld
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
