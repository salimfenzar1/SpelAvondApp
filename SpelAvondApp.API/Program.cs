using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpelAvondApp.Domain.Models;
using SpelAvondApp.Infrastructure;
using SpelAvondApp.Application;
using System.Text;
using Microsoft.AspNetCore.Builder;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SpelAvondApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Load configurations
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Configure connection strings
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var spellenDbConnectionString = builder.Configuration.GetConnectionString("SpellenDbConnection")
    ?? throw new InvalidOperationException("Connection string 'SpellenDbConnection' not found.");

// Configure the Identity database with ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

// Configure the spellen-database with SpellenDbContext
builder.Services.AddDbContext<SpellenDbContext>(options =>
    options.UseSqlServer(spellenDbConnectionString));

// Configure Identity without roles
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Load JWT settings directly from appsettings.json
string jwtIssuer = builder.Configuration["Jwt:Issuer"];
string jwtAudience = builder.Configuration["Jwt:Audience"];
string jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience) || string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT configuration values are missing.");
}

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Token validation failed: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };
});

// Add UserManager and other Identity services
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add repositories and services
builder.Services.AddScoped<ISpellenRepository, SpellenRepository>();
builder.Services.AddScoped<IBordspelService, BordspelService>();
builder.Services.AddScoped<IBordspellenAvondService, BordspellenAvondService>();
builder.Services.AddScoped<IInschrijvingService, InschrijvingService>();

// Add Hot Chocolate services for GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<QueryType>(); // Voeg je query types hier toe

// Add Swagger/OpenAPI for the REST API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter JWT with Bearer in the field. Example: \"Bearer {token}\"",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL("/api/graphql");

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
