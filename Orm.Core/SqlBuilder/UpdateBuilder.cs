using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.SqlBuilder;

internal static class UpdateBuilder
{
    public static string Update<T>(T entity, EntityMetadata entityMetadata)
    {
        var pk = entityMetadata.PrimaryKey;
        if (pk is null)
            throw new InvalidOperationException(
                $"Entity '{entityMetadata.TableName}' has no primary key defined. Update requires a primary key.");

        var setParts = new List<string>();

        foreach (var col in entityMetadata.Columns)
        {
            if (col.IsPrimaryKey && col.IsAutoIncrement)
                continue;

            var value = col.Property.GetValue(entity);

            setParts.Add(value == null
                ? $"{col.ColumnName} = NULL"
                : $"{col.ColumnName} = {ToSqlHelper.FormatValue(value)}"
            );
        }

        if (setParts.Count == 0)
            throw new InvalidOperationException($"Entity '{entityMetadata.TableName}' has no updatable columns.");

        var pkValue = pk.Property.GetValue(entity)
                      ?? throw new InvalidOperationException("Primary key value cannot be null for update.");

        var sql = $@"
UPDATE {entityMetadata.TableName}
SET {string.Join(", ", setParts)}
WHERE {pk.ColumnName} = {ToSqlHelper.FormatValue(pkValue)};
";

        return sql;
    }
}
