using MongoDB.Driver;
using StackExchange.Redis;
using TravelPlanner.Application.Services;
using TravelPlanner.Domain.Interfaces;
using TravelPlanner.Infrastructure.Mongo;
using TravelPlanner.Infrastructure.Redis;
using TravelPlanner.Infrastructure.Neo4j;
using Neo4j.Driver;
using TravelPlanner.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5059");

// Add services to the containe

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Configuration MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongoUrl = builder.Configuration.GetConnectionString("MongoDb") ?? "mongodb://localhost:27017";
    return new MongoClient(mongoUrl);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("travel_planner");
});

// Configuration Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisUrl = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
    return ConnectionMultiplexer.Connect(redisUrl);
});
builder.Services.AddSingleton<IDriver>(sp =>
{
    var neo4jUrl = builder.Configuration.GetConnectionString("Neo4j") ?? "bolt://localhost:7687";
    var user = builder.Configuration["Neo4j:Username"] ?? "neo4j";
    var password = builder.Configuration["Neo4j:Password"] ?? "password";
    Console.WriteLine(user);
    Console.WriteLine(password);
    return GraphDatabase.Driver(neo4jUrl, AuthTokens.Basic(user, password));
});
// Infrastructure
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IOfferRepository, MongoOfferRepository>();
builder.Services.AddScoped<IRecommendationRepository, Neo4jRecommendationRepository>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IOfferCreationService, OfferCreationService>();
builder.Services.AddScoped<ICityGraphService, Neo4jCityGraphService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, MongoUserRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("travel_redis"));
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<ICityGraphRepository, Neo4jCityGraphRepository>();
builder.Services.AddScoped<INotificationService, RedisNotificationService>();

// Application
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

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
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
