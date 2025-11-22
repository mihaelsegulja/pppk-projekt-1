using Orm.Core.Connection;
using Orm.Core.Mapping;
using Orm.Core.SqlGenerator;

namespace Orm.Core;

public class OrmClient : IDisposable
{
    private readonly DatabaseConnection _conn;
    private readonly EntityMapper _mapper;
    private readonly CreateTableGenerator _createTable;

    public OrmClient(string connectionString)
    {
        _conn = new DatabaseConnection(connectionString);
        _mapper = new EntityMapper();
        _createTable = new CreateTableGenerator();
    }

    public void CreateTable<T>()
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = _createTable.CreateTable(metadata);
        ExecuteNonQuery(sql);
    }

    private void ExecuteNonQuery(string sql)
    {
        using var conn = _conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    public void Dispose()
    {
        _conn.Dispose();
    }
}
