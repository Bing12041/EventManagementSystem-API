using EventManagementSystem.API.Data;
using EventManagementSystem.API.Repository;
using EventManagementSystem.API.Service;
// using Microsoft.AspNetCore.Authentication.JwtBearer; // Commented out for authentication
using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens; // Commented out for authentication
// using Azure.Identity; // Commented out for authentication
using System.Text;
using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Configuration.AzureKeyVault; // Commented out for authentication

var builder = WebApplication.CreateBuilder(args);

// Azure Key Vault configuration (commented out for authentication)
// var keyVaultUrl = "https://eventmanagementsystem.vault.azure.net/";
// builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());

// Add services to the container.
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

// Configure Authentication with JWT (commented out for authentication)
/*
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});
*/

// Add authorization policies (commented out for authentication)
/*
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
});
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// Authentication and Authorization middleware (commented out for authentication)
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();