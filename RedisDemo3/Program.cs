using Microsoft.EntityFrameworkCore;
using RedisDemo3.Cache;
using RedisDemo3.DBContext;
using RedisDemo3.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//MYSQL
var connectionString = builder.Configuration.GetConnectionString("MysqlDatabase");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A connection string named 'MysqlDatabase' is not configured.");
}
builder.Services.AddDbContext<DbContextClass>(options => options.UseMySQL(connectionString));

//REDIS
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("RedisCache"));

builder.Services.AddScoped<ICacheService, CacheService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
