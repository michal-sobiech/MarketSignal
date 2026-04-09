using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Infrastructure.Job;
using MarketSignal.Infrastructure.MarketDb;

using Microsoft.EntityFrameworkCore;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Market database
builder.Services.AddSingleton<MarketDbOptions>(_ => new MarketDbOptions {
    Host = Environment.GetEnvironmentVariable("MARKET_DB_HOST") ?? throw new ArgumentException("MARKET_DB_HOST is not set"),
    Port = int.Parse(Environment.GetEnvironmentVariable("MARKET_DB_PORT") ?? throw new ArgumentException("MARKET_DB_PORT is not set")),
    DbName = Environment.GetEnvironmentVariable("MARKET_DB_NAME") ?? throw new ArgumentException("MARKET_DB_NAME is not set"),
    UserName = Environment.GetEnvironmentVariable("MARKET_DB_USER_NAME") ?? throw new ArgumentException("MARKET_DB_USER_NAME is not set"),
    Password = Environment.GetEnvironmentVariable("MARKET_DB_PASSWORD") ?? throw new ArgumentException("MARKET_DB_PASSWORD is not set"),
});


// var marketDbOptions = builder.Services.BuildServiceProvider().GetRequiredService<MarketDbOptions>();
// Console.WriteLine($"DB: Server={marketDbOptions.Host};Port={marketDbOptions.Port};Database={marketDbOptions.DbName};Uid={marketDbOptions.UserName};Pwd={marketDbOptions.Password};AllowPublicKeyRetrieval=True;");

builder.Services.AddDbContext<MarketDbContext>((serviceProvider, options) => {
    var marketDbOptions = serviceProvider.GetRequiredService<MarketDbOptions>();
    string connectionString = $"Server={marketDbOptions.Host};Port={marketDbOptions.Port};Database={marketDbOptions.DbName};Uid={marketDbOptions.UserName};Pwd={marketDbOptions.Password};AllowPublicKeyRetrieval=True;";
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Redis
builder.Services.AddSingleton<RedisOptions>(_ => new RedisOptions {
    Host = Environment.GetEnvironmentVariable("REDIS_HOST") ?? throw new ArgumentException("REDIS_HOST is not set"),
    Port = int.Parse(Environment.GetEnvironmentVariable("REDIS_PORT") ?? throw new ArgumentException("REDIS_PORT is not set"))
});
builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider => {
    var redisOptions = serviceProvider.GetRequiredService<RedisOptions>();
    string connectionString = $"{redisOptions.Host}:{redisOptions.Port},abortConnect=false";
    return ConnectionMultiplexer.Connect(connectionString);
});
builder.Services.AddSingleton<IJobQueueProducer>(serviceProvider =>
    new RedisJobQueueProducer(
        serviceProvider.GetRequiredService<IConnectionMultiplexer>(),
        "jobs"
    )
);
builder.Services.AddSingleton<IJobStore>(serviceProvider =>
    new RedisJobStore(
        serviceProvider.GetRequiredService<IConnectionMultiplexer>(),
        "jobs"
    )
);

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<MarketDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();