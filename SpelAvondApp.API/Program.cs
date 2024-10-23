using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpelAvondApp.Domain.Models;
using SpelAvondApp.Infrastructure;
using Microsoft.AspNetCore.Builder;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpelAvondApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.api.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.api.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Check if the application is in development or production
var isDevelopment = builder.Environment.IsDevelopment();

// Connection string for the Identity database
var identityConnectionString = isDevelopment
    ? builder.Configuration.GetConnectionString("DefaultConnection")
    : Environment.GetEnvironmentVariable("IDENTITY_CONNSTR");

if (identityConnectionString == null)
    throw new InvalidOperationException("Connection string 'IDENTITY_CONNSTR' not found.");

// Connection string for the Spellen database
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



// Add UserManager and other Identity services
builder.Services.AddScoped<UserManager<ApplicationUser>>();

// Add your repositories and other services
builder.Services.AddScoped<ISpellenRepository, SpellenRepository>();

// Add Hot Chocolate services for GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<QueryType>(); // Add your query types here


// Add Swagger/OpenAPI for the REST API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Swagger and GraphQL
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();


app.MapGraphQL("/api/graphql");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
