using System.Reflection;
using Orm.Core.Attributes;
using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.Mapping;

internal class EntityMapper
{
    public EntityMetadata MapEntity(Type type)
    {
        var metadata = new EntityMetadata
        {
            RuntimeType = type,
            TableName = ResolveTableName(type),
            Columns = new List<ColumnMetadata>()
        };

        foreach (var property in type.GetProperties())
        {
            var column = BuildColumnMetadata(property);
            metadata.Columns.Add(column);

            if (column.IsPrimaryKey)
                metadata.PrimaryKey = column;
            
            if (column.IsForeignKey)
                metadata.ForeignKeys.Add(column);
        }

        return metadata.PrimaryKey == null ? 
            throw new Exception($"Entity {type.Name} must have a primary key.") : 
            metadata;
    }
    
    private static string ResolveTableName(Type type)
    {
        var tableAttr = type.GetCustomAttribute<TableAttribute>();

        if (tableAttr != null && !string.IsNullOrWhiteSpace(tableAttr.Name))
            return tableAttr.Name;

        return NamingHelper.PascalCaseToSnakeCase(type.Name);
    }
    
    private static ColumnMetadata BuildColumnMetadata(PropertyInfo property)
    {
        var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
        var pkAttr = property.GetCustomAttribute<PrimaryKeyAttribute>();
        var notNullAttr = property.GetCustomAttribute<NotNullAttribute>();
        var uniqueAttr = property.GetCustomAttribute<UniqueAttribute>();
        var defaultAttr = property.GetCustomAttribute<DefaultAttribute>();
        var fkAttr = property.GetCustomAttribute<ForeignKeyAttribute>();

        string columnName =
            columnAttr?.Name ??
            NamingHelper.PascalCaseToSnakeCase(property.Name);

        bool isPrimary = pkAttr != null;
        bool isAutoInc = pkAttr?.AutoIncrement ?? false;
        bool isNotNull = notNullAttr != null || isPrimary;
        bool isUnique = uniqueAttr != null;
        bool isForeignKey = fkAttr != null;
        Type? fkType = fkAttr?.ReferencedType;

        string dbType = ToSqlHelper.ToDbType(property.PropertyType);

        return new ColumnMetadata
        {
            Property = property,
            ColumnName = columnName,
            RuntimeType = property.PropertyType,
            DbType = dbType,
            IsPrimaryKey = isPrimary,
            IsAutoIncrement = isAutoInc,
            IsNotNull = isNotNull,
            IsUnique = isUnique,
            DefaultValue = defaultAttr?.Value,
            IsForeignKey = isForeignKey,
            ForeignKeyReferenceType = fkType,
        };
    }
}