using MarketSignal.Contracts.Instrument.RawData;
using MarketSignal.Contracts.Job.Queue;
using MarketSignal.Contracts.Job.Store;
using MarketSignal.Core.EnvVar;
using MarketSignal.Core.Indicator;
using MarketSignal.Core.Instrument.RawData;
using MarketSignal.Infrastructure.Instrument.RawData;
using MarketSignal.Infrastructure.Job;
using MarketSignal.Infrastructure.MarketDb;
using MarketSignal.Worker;
using MarketSignal.Worker.Indicator;
using MarketSignal.Worker.Instrument.RawData;

using Microsoft.EntityFrameworkCore;

using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

// TODO add env vars util for checking if a value is set
// ==========
// Market database
// ==========

builder.Services.AddSingleton<MarketDbOptions>(_ => new MarketDbOptions {
    Host = EnvVarUtils.RequireEnvVar("MARKET_DB_HOST"),
    Port = int.Parse(EnvVarUtils.RequireEnvVar("MARKET_DB_PORT")),
    DbName = EnvVarUtils.RequireEnvVar("MARKET_DB_NAME"),
    UserName = EnvVarUtils.RequireEnvVar("MARKET_DB_USER_NAME"),
    Password = EnvVarUtils.RequireEnvVar("MARKET_DB_PASSWORD")
});
builder.Services.AddDbContext<MarketDbContext>((serviceProvider, options) => {
    var marketDbOptions = serviceProvider.GetRequiredService<MarketDbOptions>();
    string connectionString = $"Server={marketDbOptions.Host};Port={marketDbOptions.Port};Database={marketDbOptions.DbName};Uid={marketDbOptions.UserName};Pwd={marketDbOptions.Password};AllowPublicKeyRetrieval=True";
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// ==========
// Redis
// ==========

builder.Services.AddSingleton<RedisOptions>(_ => new RedisOptions {
    Host = EnvVarUtils.RequireEnvVar("REDIS_HOST"),
    Port = int.Parse(EnvVarUtils.RequireEnvVar("REDIS_PORT"))
});
builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider => {
    var redisOptions = serviceProvider.GetRequiredService<RedisOptions>();
    string connectionString = $"{redisOptions.Host}:{redisOptions.Port},abortConnect=false";
    return ConnectionMultiplexer.Connect(connectionString);
});
builder.Services.AddSingleton<IJobQueueConsumer>(serviceProvider =>
    new RedisJobQueueConsumer(
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

// ==========
// App
// ==========

// Alpha Vantage
builder.Services.AddSingleton<AVInstrumentIdMapper>();
builder.Services.AddSingleton<AVInstrumentRawDataProviderOptions>(_ => new AVInstrumentRawDataProviderOptions {
    BaseUrl = EnvVarUtils.RequireEnvVar("ALPHA_VANTAGE_BASE_URL"),
    ApiKey = EnvVarUtils.RequireEnvVar("ALPHA_VANTAGE_API_KEY")
});


builder.Services.AddSingleton<IInstrumentRawDataProvider, AVInstrumentRawDataProvider>();
builder.Services.AddSingleton<InstrumentRawDataUpdater>();
builder.Services.AddSingleton<UpdateInstrumentRawDataJobHandler>();
builder.Services.AddSingleton<IndicatorValuesUpdater>();
builder.Services.AddSingleton<UpdateIndicatorValuesJobHandler>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
