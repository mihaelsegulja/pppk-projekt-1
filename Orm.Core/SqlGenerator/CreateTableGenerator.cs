using Orm.Core.Models;
using Orm.Core.Utils;
using System.Text;

namespace Orm.Core.SqlGenerator;

internal class CreateTableGenerator
{
    public static string CreateTable(EntityMetadata entityMetadata, bool ifNotExists = true)
    {
        var sql = new StringBuilder();
        var condition = ifNotExists ? "IF NOT EXISTS" : string.Empty;

        sql.AppendLine($"CREATE TABLE {condition} {entityMetadata.TableName} (");

        var columnDefinitions = new List<string>();
        foreach (var column in entityMetadata.Columns)
        {
            var columnDef = new StringBuilder();
            columnDef.Append($"{column.ColumnName} {column.DbType}");
            if (column.IsAutoIncrement)
            {
                columnDef.Append(" SERIAL");
            }
            if (column.IsPrimaryKey)
            {
                columnDef.Append(" PRIMARY KEY");
            }
            if (column.IsNotNull)
            {
                columnDef.Append(" NOT NULL");
            }
            if (column.IsUnique)
            {
                columnDef.Append(" UNIQUE");
            }
            if (column.DefaultValue != null)
            {
                columnDef.Append($" DEFAULT {ToSqlHelper.FormatValue(column.DefaultValue)}");
            }

            columnDefinitions.Add(columnDef.ToString());
        }

        sql.AppendLine(string.Join(",\n", columnDefinitions));
        sql.AppendLine(");");

        return sql.ToString();
    }
}
