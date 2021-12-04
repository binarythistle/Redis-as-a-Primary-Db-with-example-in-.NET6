using CacheService.Data;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IConnectionMultiplexer>(opt => 
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("DockerRedisConnection")));

builder.Services.AddScoped<IPlatformRepo, RedisPlatformRepo>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
