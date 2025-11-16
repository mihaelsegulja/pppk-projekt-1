using dotenv.net;
using Orm.Console;
using Orm.Core.Database;
using Orm.Core.Mapping;

Console.WriteLine("Testing PostgreSQL connection...");

DotEnv.Load();

try
{
    var connStr = DatabaseConfig.GetConnectionString();
    using var db = new DatabaseConnection(connStr);
    using var conn = db.Open();
    Console.WriteLine("Connection successful!");
    EntityMapper w =  new EntityMapper();
    w.MapEntity(typeof(Patient));
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
}