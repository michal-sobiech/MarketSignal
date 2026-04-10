namespace MarketSignal.Core.EnvVar;

public static class EnvVarUtils {

    public static string RequireEnvVar(string name) {
        return Environment.GetEnvironmentVariable(name)
            ?? throw new ArgumentException($"Environment variable \"{name}\" is not set");
    }

}