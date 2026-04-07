using MarketSignal.Infrastructure.Job;
using MarketSignal.Worker;

using Microsoft.Extensions.Options;

using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));

builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider => {
    var options = serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;
    return ConnectionMultiplexer.Connect(options.ConnectionString);
});

builder.Services.AddSingleton<RedisJobQueueConsumer>();
builder.Services.AddSingleton<RedisJobStore>();

var host = builder.Build();
host.Run();
