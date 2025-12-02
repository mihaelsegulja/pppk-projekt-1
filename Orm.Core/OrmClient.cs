using Orm.Core.Connection;
using Orm.Core.Mapping;
using Orm.Core.SqlGenerator;

namespace Orm.Core;

public class OrmClient : IDisposable, IOrmClient
{
    private readonly DatabaseConnection _conn;
    private readonly EntityMapper _mapper;

    public OrmClient(string connectionString)
    {
        _conn = new DatabaseConnection(connectionString);
        _mapper = new EntityMapper();
    }

    public void CreateTable<T>()
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = CreateTableGenerator.CreateTable(metadata);
        ExecuteNonQuery(sql);
    }

    public void Insert<T>(T entity)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = InsertGenerator.Insert(entity, metadata);
        ExecuteNonQuery(sql);
    }

    public void DeleteById<T>(object id)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var pk = metadata.PrimaryKey;
        var sql = DeleteGenerator.Delete(metadata.TableName, pk.ColumnName, id);
        ExecuteNonQuery(sql);
    }

    public void DeleteWhere<T>(string columnName, object value)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = DeleteGenerator.Delete(metadata.TableName, columnName, value);
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
        GC.SuppressFinalize(this);
    }
}
