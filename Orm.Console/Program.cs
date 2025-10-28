using dotenv.net;
using Orm.Core.Database;

Console.WriteLine("Testing PostgreSQL connection...");

DotEnv.Load();

try
{
    using var db = new DatabaseConnection(DatabaseConfig.getConnectionString());
    using var conn = db.Open();
    Console.WriteLine("Connection successful!");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
}