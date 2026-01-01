using System.Data;
using Orm.Core.ChangeTracking;
using Orm.Core.Connection;
using Orm.Core.Mapping;
using Orm.Core.Models;
using Orm.Core.SqlBuilder;
using Orm.Core.Utils;

namespace Orm.Core;

public class OrmContext : IDisposable, IOrmContext
{
    private readonly DatabaseConnection _conn;
    private readonly EntityMapper _mapper;
    private readonly ChangeTracker _changeTracker;

    public OrmContext(string connectionString)
    {
        _conn = new DatabaseConnection(connectionString);
        _mapper = new EntityMapper();
        _changeTracker = new ChangeTracker();
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
        var pk = metadata.PrimaryKey;

        var sql = InsertBuilder.Insert(entity, metadata);

        using var conn = _conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        if (pk.IsAutoIncrement)
        {
            var result = cmd.ExecuteScalar();
            if (result != null)
            {
                var targetType = Nullable.GetUnderlyingType(pk.RuntimeType) ?? pk.RuntimeType;
                var converted = Convert.ChangeType(result, targetType);
                pk.Property.SetValue(entity, converted);
            }
        }
        else
        {
            cmd.ExecuteNonQuery();
        }

        _changeTracker.Track(entity, metadata);
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
    
    public IEnumerable<T> GetAll<T>(SelectOptions? options = null, Action<IncludeOptions>? include = null) where T : new()
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = SelectBuilder.Select(metadata, options);
        var entities = ExecuteQuery<T>(sql, metadata);

        if (include == null) 
            return entities;
        
        var includeOptions = new IncludeOptions();
        include(includeOptions);
        ApplyIncludes(entities, metadata, includeOptions);

        return entities;
    }

    public T? GetById<T>(object id, Action<IncludeOptions>? include = null) where T : new()
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = SelectBuilder.SelectById(metadata, id);
        var entity = ExecuteQuerySingle<T>(sql, metadata);
        if (entity == null || include == null)
            return entity;

        var includeOptions = new IncludeOptions();
        include(includeOptions);

        ApplyIncludes(new[] { entity }, metadata, includeOptions);

        return entity;
    }
    
    public void Update<T>(T entity)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = UpdateBuilder.Update(entity, metadata);
        ExecuteNonQuery(sql);
    }
    
    public void AddColumn<T>(string propertyName)
    {
        var metadata = _mapper.MapEntity(typeof(T));

        var column = metadata.Columns
                         .FirstOrDefault(c => c.Property.Name == propertyName)
                     ?? throw new InvalidOperationException(
                         $"Property {propertyName} not found on {typeof(T).Name}");

        var sql = AlterTableBuilder.AddColumn(metadata, column);
        ExecuteNonQuery(sql);
    }

    public void DropColumn<T>(string columnName)
    {
        var metadata = _mapper.MapEntity(typeof(T));
        var sql = AlterTableBuilder.DropColumn(metadata, columnName);
        ExecuteNonQuery(sql);
    }
    
    public void Remove<T>(T entity)
    {
        _changeTracker.MarkDeleted(entity!);
    }
    
    public int SaveChanges()
    {
        int affected = 0;
        
        foreach (var tracked in _changeTracker.DeletedEntities().ToList())
        {
            var metadata = tracked.Metadata;
            var pk = metadata.PrimaryKey
                     ?? throw new InvalidOperationException("Entity has no PK");

            var id = pk.Property.GetValue(tracked.Entity);

            var sql = DeleteBuilder.DeleteById(metadata, id!);
            ExecuteNonQuery(sql);

            _changeTracker.Detach(tracked.Entity);
            affected++;
        }

        foreach (var tracked in _changeTracker.TrackedEntities())
        {
            if (tracked.State != EntityState.Modified)
                continue;

            var changes = _changeTracker.DetectChanges(tracked.Entity);
            if (changes.Count == 0)
                continue;

            var sql = UpdateBuilder.UpdatePartial(
                tracked.Entity,
                tracked.Metadata,
                changes
            );

            ExecuteNonQuery(sql);

            _changeTracker.AcceptChanges(tracked);
            affected++;
        }

        return affected;
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
            _changeTracker.Track(entity, metadata);
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
        _changeTracker.Track(entity, metadata);
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
    
    private void ApplyIncludes<T>(IEnumerable<T> entities, EntityMetadata rootMetadata, IncludeOptions includeOptions)
    {
        foreach (var includeType in includeOptions.IncludedTypes)
        {
            LoadReference(entities, rootMetadata, includeType);
        }
    }

    private void LoadReference<T>(IEnumerable<T> entities, EntityMetadata rootMetadata, Type includeType)
    {
        // Find FK column
        var fkColumn = rootMetadata.ForeignKeys
            .FirstOrDefault(fk => fk.ForeignKeyReferenceType == includeType);

        if (fkColumn == null)
            throw new InvalidOperationException(
                $"No foreign key from {rootMetadata.RuntimeType.Name} to {includeType.Name}");

        // Find navigation property
        var navigationProperty = rootMetadata.RuntimeType
            .GetProperties()
            .FirstOrDefault(p => p.PropertyType == includeType);

        if (navigationProperty == null)
            throw new InvalidOperationException(
                $"Navigation property {includeType.Name} not found on {rootMetadata.RuntimeType.Name}");

        // Collect FK values
        var ids = entities
            .Select(e => fkColumn.Property.GetValue(e))
            .Where(v => v != null)
            .Distinct()
            .ToList();

        if (ids.Count == 0)
            return;

        // Build SelectOptions with IN
        var referencedMetadata = _mapper.MapEntity(includeType);

        var opts = new SelectOptions();
        opts.WhereGroups.Add(new WhereGroup
        {
            Conditions =
            {
                new WhereCondition(
                    referencedMetadata.PrimaryKey.ColumnName,
                    SqlConditionOperatorType.In,
                    ids)
            }
        });

        var sql = SelectBuilder.Select(referencedMetadata, opts);
        var referencedEntities = ExecuteQueryRaw(sql, referencedMetadata);

        // Index by PK
        var pk = referencedMetadata.PrimaryKey;
        var lookup = referencedEntities.ToDictionary(
            e => pk.Property.GetValue(e)!
        );

        // Assign navigation property
        foreach (var entity in entities)
        {
            var fkValue = fkColumn.Property.GetValue(entity);
            if (fkValue != null && lookup.TryGetValue(fkValue, out var referenced))
            {
                navigationProperty.SetValue(entity, referenced);
            }
        }
    }
    
    private List<object> ExecuteQueryRaw(string sql, EntityMetadata metadata)
    {
        var results = new List<object>();

        using var conn = _conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var entity = Activator.CreateInstance(metadata.RuntimeType)!;
            MapRowToEntity(reader, metadata, entity);
            results.Add(entity);
        }

        return results;
    }
    #endregion
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
