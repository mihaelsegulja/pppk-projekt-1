using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.SqlBuilder;

internal static class InsertBuilder
{
    public static string Insert<T>(T? entity, EntityMetadata metadata)
    {
        var columnNames = new List<string>();
        var columnValues = new List<string>();

        foreach (var column in metadata.Columns)
        {
            if (column.IsPrimaryKey && column.IsAutoIncrement)
                continue;

            var value = column.Property.GetValue(entity);
            var sqlValue = ToSqlHelper.FormatValue(value);

            columnNames.Add(column.ColumnName);
            columnValues.Add(sqlValue);
        }

        var names = string.Join(',', columnNames);
        var values = string.Join(',', columnValues);

        return $"INSERT INTO {metadata.TableName} ({names}) VALUES ({values});";
    }
}
