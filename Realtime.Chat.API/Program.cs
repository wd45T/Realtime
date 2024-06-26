using Realtime.Chat.Common.TransportLayer.Utilities;
using Realtime.Chat.Service.Implementations;
using Realtime.Chat.Service.Interfaces;
using Realtime.Engine.Repositories.Implementations;
using Realtime.Engine.Repositories.Interfaces;
using Realtime.Engine.Services;
using Realtime.Engine.Services.Implementations;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IRealTimeEventService, LongPollingEventService>();
builder.Services.AddScoped<ILongPollingRepository, LongPollingByRedisRepository>();
builder.Services.AddScoped<IDataConverter, MsgPackDataConverter>();

var redisUrl = builder.Configuration.GetValue<string>("RedisUrl");

Console.WriteLine($"RedisUrl: {redisUrl}. Environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");

try
{
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisUrl));
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
