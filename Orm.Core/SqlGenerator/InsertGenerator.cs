using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.SqlGenerator;

internal static class InsertGenerator
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

        var names = string.Join(",\n", columnNames);
        var values = string.Join(",\n", columnValues);

        return $"INSERT INTO {metadata.TableName} ({names}) VALUES ({values});";
    }
}
