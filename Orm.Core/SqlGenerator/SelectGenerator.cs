using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.SqlGenerator;

internal static class SelectGenerator
{
    public static string SelectAll(EntityMetadata metadata)
    {
        var columns = string.Join(", ",
            metadata.Columns.Select(c => c.ColumnName));

        return $"SELECT {columns} FROM {metadata.TableName};";
    }

    public static string SelectById(EntityMetadata metadata, object id)
    {
        var pk = metadata.PrimaryKey 
                 ?? throw new InvalidOperationException(
                     $"Entity {metadata.RuntimeType.Name} has no primary key.");

        var formattedId = ToSqlHelper.FormatValue(id);

        var columns = string.Join(", ",
            metadata.Columns.Select(c => c.ColumnName));

        return $"SELECT {columns} FROM {metadata.TableName} WHERE {pk.ColumnName} = {formattedId};";
    }
}
