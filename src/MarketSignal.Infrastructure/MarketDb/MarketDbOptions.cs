namespace MarketSignal.Infrastructure.MarketDb;

public class MarketDbOptions {
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string DbName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}