using dotenv.net;
using Orm.Core.Database;

Console.WriteLine("Testing PostgreSQL connection...");

DotEnv.Load();

try
{
    var connStr = DatabaseConfig.GetConnectionString();
    using var db = new DatabaseConnection(connStr);
    using var conn = db.Open();
    Console.WriteLine("Connection successful!");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
}