using System.Data;
using Orm.Core.Connection;
using Orm.Core.Mapping;
using Orm.Core.Models;
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
    
    public IEnumerable<T> GetAll<T>() where T : new()
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = SelectGenerator.SelectAll(metadata);
        return ExecuteQuery<T>(sql, metadata);
    }

    public T? GetById<T>(object id) where T : new()
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = SelectGenerator.SelectById(metadata, id);
        return ExecuteQuerySingle<T>(sql, metadata);
    }
    
    public void Update<T>(T entity)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = UpdateGenerator.Update(entity, metadata);
        ExecuteNonQuery(sql);
    }

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
        // we rely on SelectGenerator selecting columns in metadata.Columns order,
        // so reader ordinal 0 == metadata.Columns[0], etc.
        while (reader.Read())
        {
            var entity = new T();
            MapReaderRowToEntity(reader, metadata, entity);
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
        MapReaderRowToEntity(reader, metadata, entity);
        return entity;
    }
    
    private static void MapReaderRowToEntity<T>(IDataRecord reader, EntityMetadata metadata, T entity)
    {
        // We assume SelectGenerator created the SELECT in the same order as metadata.Columns.
        // So we can read by ordinal index (fast) and avoid GetOrdinal lookups.
        for (int i = 0; i < metadata.Columns.Count; i++)
        {
            var colMeta = metadata.Columns[i];
            // Guard: ensure the reader actually has this column index
            if (i >= reader.FieldCount)
                continue;

            if (reader.IsDBNull(i))
            {
                // leave default/null in the CLR object
                continue;
            }

            object raw = reader.GetValue(i);
            var targetType = colMeta.RuntimeType;
            var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

            object? converted;

            if (underlying.IsEnum)
            {
                // raw might be int or string
                if (raw is string s)
                    converted = Enum.Parse(underlying, s);
                else
                    converted = Enum.ToObject(underlying, raw);
            }
            else if (underlying == typeof(Guid))
            {
                if (raw is Guid g) converted = g;
                else converted = Guid.Parse(raw.ToString()!);
            }
            else if (underlying == typeof(char))
            {
                if (raw is char rc) converted = rc;
                else if (raw is string rs && rs.Length > 0) converted = rs[0];
                else converted = Convert.ChangeType(raw, underlying);
            }
            else if (underlying == typeof(bool))
            {
                // DBs may represent booleans as bool or numeric
                if (raw is bool b) converted = b;
                else converted = Convert.ToBoolean(raw);
            }
            else if (underlying == typeof(DateTimeOffset))
            {
                if (raw is DateTimeOffset dto) converted = dto;
                else if (raw is DateTime dt) converted = new DateTimeOffset(dt);
                else converted = DateTimeOffset.Parse(raw.ToString()!);
            }
            else
            {
                // Generic conversion for numbers, strings, DateTime, etc.
                converted = Convert.ChangeType(raw, underlying);
            }

            // Set property. Reflection will box/unbox as needed.
            colMeta.Property.SetValue(entity, converted);
        }
    }
    
    public void Dispose()
    {
        _conn.Dispose();
        GC.SuppressFinalize(this);
    }
}
