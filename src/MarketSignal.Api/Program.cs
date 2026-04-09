using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Infrastructure.Job;
using MarketSignal.Worker;

using Microsoft.Extensions.Options;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();

builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));

builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider => {
    var options = serviceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;
    return ConnectionMultiplexer.Connect(options.ConnectionString);
});

builder.Services.AddSingleton<IJobQueueConsumer, RedisJobQueueConsumer>();
builder.Services.AddSingleton<IJobStore, RedisJobStore>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();