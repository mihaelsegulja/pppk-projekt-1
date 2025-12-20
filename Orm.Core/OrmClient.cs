using System.Data;
using Orm.Core.Connection;
using Orm.Core.Mapping;
using Orm.Core.Models;
using Orm.Core.SqlBuilder;
using Orm.Core.Utils;

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
        var sql = CreateTableBuilder.CreateTable(metadata);
        ExecuteNonQuery(sql);
    }

    public void Insert<T>(T entity)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = InsertBuilder.Insert(entity, metadata);
        ExecuteNonQuery(sql);
    }

    public void DeleteById<T>(object id)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = DeleteBuilder.DeleteById(metadata, id);
        ExecuteNonQuery(sql);
    }

    public void Delete<T>(DeleteOptions? options =  null)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = DeleteBuilder.Delete(metadata, options);
        ExecuteNonQuery(sql);
    }
    
    public IEnumerable<T> GetAll<T>(SelectOptions? options = null) where T : new()
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = SelectBuilder.Select(metadata, options);
        return ExecuteQuery<T>(sql, metadata);
    }

    public T? GetById<T>(object id) where T : new()
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = SelectBuilder.SelectById(metadata, id);
        return ExecuteQuerySingle<T>(sql, metadata);
    }
    
    public void Update<T>(T entity)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = UpdateBuilder.Update(entity, metadata);
        ExecuteNonQuery(sql);
    }

    #region HELPERS
    private void ExecuteNonQuery(string sql)
    {
        using var conn = _conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
    
    private IEnumerable<T> ExecuteQuery<T>(string sql, EntityMetadata metadata) where T : new()
    {
        var results = new List<T>();

        using var conn = _conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            var entity = new T();
            MapRowToEntity(reader, metadata, entity);
            results.Add(entity);
        }

        return results;
    }
    
    private T? ExecuteQuerySingle<T>(string sql, EntityMetadata metadata) where T : new()
    {
        using var conn = _conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
            return default;

        var entity = new T();
        MapRowToEntity(reader, metadata, entity);
        return entity;
    }
    
    private static void MapRowToEntity<T>(IDataRecord reader, EntityMetadata metadata, T entity)
    {
        for (int i = 0; i < metadata.Columns.Count; i++)
        {
            var colMeta = metadata.Columns[i];
            if (i >= reader.FieldCount)
                continue;

            if (reader.IsDBNull(i))
                continue;

            var raw = reader.GetValue(i);
            var converted = DbValueConverter.ConvertValue(raw, colMeta.RuntimeType);

            colMeta.Property.SetValue(entity, converted);
        }
    }
    #endregion
    
    public void Dispose()
    {
        _conn.Dispose();
        GC.SuppressFinalize(this);
    }
}
