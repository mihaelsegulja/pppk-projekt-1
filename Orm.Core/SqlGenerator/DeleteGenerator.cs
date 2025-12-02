using Orm.Core.Utils;

namespace Orm.Core.SqlGenerator;

internal class DeleteGenerator
{
    public static string Delete(string tableName, string? columnName, object? value)
    {

        if (string.IsNullOrEmpty(columnName) || value is null)
        {
            return $"DELETE FROM {tableName};";
        }

        return $"DELETE FROM {tableName} WHERE {columnName} = {ToSqlHelper.FormatValue(value)};";
    }
}
