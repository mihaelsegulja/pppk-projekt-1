using System.Text;
using Orm.Core.Mapping;
using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.SqlBuilder;

internal static class CreateTableBuilder
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
        
        var fkConstraints = new List<string>();

        foreach (var fk in entityMetadata.ForeignKeys)
        {
            var referencedTable = NamingHelper.PascalCaseToSnakeCase(fk.ForeignKeyReferenceType!.Name);

            var refMetadata = new EntityMapper().MapEntity(fk.ForeignKeyReferenceType);
            var referencedColumn = refMetadata.PrimaryKey.ColumnName;

            fkConstraints.Add(
                $"FOREIGN KEY ({fk.ColumnName}) REFERENCES {referencedTable}({referencedColumn})"
            );
        }

        if (fkConstraints.Count != 0)
            columnDefinitions.AddRange(fkConstraints);
        
        sql.AppendLine(string.Join(",\n", columnDefinitions));
        sql.AppendLine(");");

        return sql.ToString();
    }
}
