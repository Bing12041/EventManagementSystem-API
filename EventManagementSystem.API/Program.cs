using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EventManagementSystem.API.Data;
using EventManagementSystem.API.Repository;
using EventManagementSystem.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var keyVaultUri = "https://eventdbkeyvault.vault.azure.net/";

var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());

string jwtSecret;
try
{
    var secret = await secretClient.GetSecretAsync("jwt-secret");
    jwtSecret = secret.Value.Value;
}
catch (Exception ex)
{
    // Log the exception or handle it appropriately
    Console.WriteLine($"Error accessing Azure Key Vault: {ex.Message}");
    // For demonstration, we'll throw an exception here to halt startup if secret retrieval fails
    throw;
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventDB")));

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler =
    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRSVPRepository, RSVPRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IRSVPService, RSVPService>();

// Adding Cors to allow front end
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5180",
                    "https://delightful-mud-0f52c600f.4.azurestaticapps.net/"
                ) // specify your React dev origin
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // allow credentials
        }
    );
});

// Add Service for Jwt Bearer Auth
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();