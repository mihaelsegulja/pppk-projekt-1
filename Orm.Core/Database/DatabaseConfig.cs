namespace Orm.Core.Database;

public class DatabaseConfig
{
    public static string EnvKey => "PPPK_CONN";
    public static string getConnectionString(string? overrideValue = null)
    {
        var conn = overrideValue ?? Environment.GetEnvironmentVariable(EnvKey);
        if (string.IsNullOrWhiteSpace(conn))
            throw new InvalidOperationException($"Environment variable '{EnvKey}' not set.");
        return conn;
    }
}
