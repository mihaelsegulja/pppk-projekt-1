using dotenv.net;
using Orm.Core;

Console.WriteLine("Testing PostgreSQL connection...");

DotEnv.Load();

try
{
    var connStr = Environment.GetEnvironmentVariable("PPPK_CONN")
        ?? throw new Exception("Missing conn string");

    using var db = new OrmClient(connStr);
    Console.WriteLine("Connection successful!");
    
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
}