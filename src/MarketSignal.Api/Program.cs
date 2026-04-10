using System.Text.Json.Serialization;

using MarketSignal.Contracts.Indicator;
using MarketSignal.Contracts.Indicator.Spec;
using MarketSignal.Contracts.Instrument;
using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Core;
using MarketSignal.Core.EnvVar;
using MarketSignal.Core.Indicator;
using MarketSignal.Infrastructure.Indicator;
using MarketSignal.Infrastructure.Instrument.Spec;
using MarketSignal.Infrastructure.Job;
using MarketSignal.Infrastructure.MarketDb;

using Microsoft.EntityFrameworkCore;

using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Market database
builder.Services.AddSingleton<MarketDbOptions>(_ => new MarketDbOptions {
    Host = EnvVarUtils.RequireEnvVar("MARKET_DB_HOST"),
    Port = int.Parse(EnvVarUtils.RequireEnvVar("MARKET_DB_PORT")),
    DbName = EnvVarUtils.RequireEnvVar("MARKET_DB_NAME"),
    UserName = EnvVarUtils.RequireEnvVar("MARKET_DB_USER_NAME"),
    Password = EnvVarUtils.RequireEnvVar("MARKET_DB_PASSWORD")
});
builder.Services.AddDbContext<MarketDbContext>((serviceProvider, options) => {
    var marketDbOptions = serviceProvider.GetRequiredService<MarketDbOptions>();
    string connectionString = $"Server={marketDbOptions.Host};Port={marketDbOptions.Port};Database={marketDbOptions.DbName};Uid={marketDbOptions.UserName};Pwd={marketDbOptions.Password};AllowPublicKeyRetrieval=True;";
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<IInstrumentSpecRepository, EfcoreInstrumentSpecRepository>();
builder.Services.AddScoped<IIndicatorSpecRepository, EfcoreIndicatorSpecRepository>();
builder.Services.AddScoped<IIndicatorRepository, EfcoreIndicatorRepository>();

// Redis
builder.Services.AddSingleton<RedisOptions>(_ => new RedisOptions {
    Host = EnvVarUtils.RequireEnvVar("REDIS_HOST"),
    Port = int.Parse(EnvVarUtils.RequireEnvVar("REDIS_PORT"))
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

// App
builder.Services.AddScoped<IndicatorService>();
builder.Services.AddSingleton<KeyValuePairsStringParser>();
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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