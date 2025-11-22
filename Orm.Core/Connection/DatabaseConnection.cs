using Npgsql;
using System.Data;

namespace Orm.Core.Connection;

internal class DatabaseConnection : IDisposable
{
    private readonly string _connectionString;
    private NpgsqlConnection? _connection;

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection Open()
    {
        _connection ??= new NpgsqlConnection(_connectionString);

        if (_connection.State != ConnectionState.Open)
            _connection.Open();

        return _connection;
    }

    public void Close()
    {
        if (_connection?.State == ConnectionState.Open)
            _connection.Close();
    }

    public void Dispose()
    {
        Close();
        _connection?.Dispose();
    }
}
