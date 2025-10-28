using System.Data;
using eventcollectingservice.Repositories;
using eventcollectingservice.Services;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add controllers
builder.Services.AddControllers();

// Configure MySQL Connection
// builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(builder.Configuration.GetConnectionString("")));
// builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IBetCityService, BetCityService>();
builder.Services.AddScoped<IFutbol24Service, Futbol24Service>();

// Register repositories
builder.Services.AddScoped<IBetCityRepository, BetCityRepository>();
builder.Services.AddScoped<IFutbol24Repository, Futbol24Repository>();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();
