using System.Text;
using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.SqlBuilder;

internal static class SelectBuilder
{
    public static string Select(EntityMetadata metadata, SelectOptions? options = null)
    {
        var sb = new StringBuilder();

        sb.Append("SELECT ");
        sb.Append(string.Join(", ", metadata.Columns.Select(c => c.ColumnName)));
        sb.Append($" FROM {metadata.TableName}");
        
        if (options == null)
            return sb.Append(";").ToString();

        if (options.WhereGroups.Count != 0)
        {
            sb.Append(" WHERE ");

            sb.Append(string.Join(" AND ",
                options.WhereGroups.Select(group =>
                {
                    var joinOp = ToSqlHelper.ToSqlLogicalOperator(group.Operator);

                    var conditions = string.Join($" {joinOp} ",
                        group.Conditions.Select(c =>
                            $"{c.ColumnName} {ToSqlHelper.ToSqlConditionalOperator(c.Operator)} {ToSqlHelper.FormatValue(c.Value)}"));

                    return $"({conditions})";
                })));
        }

        if (options.OrderBy.Count != 0)
        {
            sb.Append(" ORDER BY ");
            sb.Append(string.Join(", ",
                options.OrderBy.Select(o =>
                    $"{o.ColumnName} {ToSqlHelper.ToSqlOrder(o.OrderBy)}")));
        }
        
        if (options.Limit.HasValue)
            sb.Append($" LIMIT {options.Limit.Value}");

        if (options.Offset.HasValue)
            sb.Append($" OFFSET {options.Offset.Value}");

        return sb.Append(";").ToString();
    }
    
    public static string SelectById(EntityMetadata metadata, object id)
    {
        if (metadata.PrimaryKey == null)
            throw new InvalidOperationException(
                $"Entity {metadata.RuntimeType.Name} has no primary key.");

        var group = new WhereGroup();
        group.Conditions.Add(
            new WhereCondition(
                metadata.PrimaryKey.ColumnName,
                SqlConditionOperatorType.Eq,
                id));

        var opts = new SelectOptions();
        opts.WhereGroups.Add(group);

        return Select(metadata, opts);
    }
}
