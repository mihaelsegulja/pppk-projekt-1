namespace Orm.Core.Database;

public class DatabaseConfig
{
    public static string EnvKey => "PPPK_CONN";
    public static string GetConnectionString(string? overrideValue = null)
    {
        var conn = overrideValue ?? Environment.GetEnvironmentVariable(EnvKey);
        
        return string.IsNullOrWhiteSpace(conn) ? 
            throw new InvalidOperationException($"Environment variable '{EnvKey}' not set.") : 
            conn;
    }
}
