using System.Text;
using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.SqlBuilder;

internal static class AlterTableBuilder
{
    public static string AddColumn(EntityMetadata metadata, ColumnMetadata column)
    {
        var columnDef = new StringBuilder();
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
        
        var dbType = column.DbType;

        return
            $"ALTER TABLE {metadata.TableName} " +
            $"ADD COLUMN {column.ColumnName} {dbType} {columnDef};";
    }

    public static string DropColumn(EntityMetadata metadata, string columnName)
    {
        return
            $"ALTER TABLE {metadata.TableName} " +
            $"DROP COLUMN {columnName};";
    }

    public static string RenameColumn(EntityMetadata metadata, string oldName, string newName)
    {
        return
            $"ALTER TABLE {metadata.TableName} " +
            $"RENAME COLUMN {oldName} TO {newName};";
    }
}