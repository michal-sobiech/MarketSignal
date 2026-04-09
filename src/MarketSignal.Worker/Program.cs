using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Infrastructure.Job;
using MarketSignal.Worker;

using Microsoft.Extensions.Options;

using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

// Market database
// builder.Services.AddSingleton<>

// Redis
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider => {
    var options = serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;
    return ConnectionMultiplexer.Connect(options.ConnectionString);
});
builder.Services.AddSingleton<IJobQueueConsumer, RedisJobQueueConsumer>();
builder.Services.AddSingleton<IJobStore, RedisJobStore>();

var host = builder.Build();
host.Run();
