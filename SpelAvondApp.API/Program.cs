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
    .AddJsonFile("appsettings.api.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.api.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Check if the application is in development or production
var isDevelopment = builder.Environment.IsDevelopment();

// Configure connection strings
var identityConnectionString = isDevelopment
    ? builder.Configuration.GetConnectionString("DefaultConnection")
    : Environment.GetEnvironmentVariable("IDENTITY_CONNSTR");

if (identityConnectionString == null)
    throw new InvalidOperationException("Connection string 'IDENTITY_CONNSTR' not found.");

var spellenDbConnectionString = isDevelopment
    ? builder.Configuration.GetConnectionString("SpellenDbConnection")
    : Environment.GetEnvironmentVariable("SPELLEN_CONNSTR");

if (spellenDbConnectionString == null)
    throw new InvalidOperationException("Connection string 'SPELLEN_CONNSTR' not found.");

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


string jwtIssuer = isDevelopment ? builder.Configuration["Jwt:Issuer"] : Environment.GetEnvironmentVariable("JWT_ISSUER");
string jwtAudience = isDevelopment ? builder.Configuration["Jwt:Audience"] : Environment.GetEnvironmentVariable("JWT_AUDIENCE");
string jwtKey = isDevelopment ? builder.Configuration["Jwt:Key"] : Environment.GetEnvironmentVariable("JWT_KEY");

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

// Configure Swagger and GraphQL
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication(); // Add Authentication Middleware
app.UseAuthorization(); // Add Authorization Middleware

app.MapGraphQL("/api/graphql");

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
