using System.Text;
using Orm.Core.Models;
using Orm.Core.Utils;

namespace Orm.Core.SqlBuilder;

internal static class DeleteBuilder
{
    public static string Delete(EntityMetadata metadata, DeleteOptions? options = null)
    {
        var sb = new StringBuilder();
        sb.Append($"DELETE FROM {metadata.TableName}");

        if (options == null)
            return sb.Append(';').ToString();
        
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

        return sb.Append(';').ToString();
    }
    
    public static string DeleteById(EntityMetadata metadata, object id)
    {
        if (metadata.PrimaryKey == null)
            throw new InvalidOperationException(
                $"Entity {metadata.RuntimeType.Name} has no primary key.");

        var group = new WhereGroup();
        group.Conditions.Add(new WhereCondition(
            metadata.PrimaryKey.ColumnName,
            SqlConditionOperatorType.Eq,
            id));

        var opts = new DeleteOptions();
        opts.WhereGroups.Add(group);

        return Delete(metadata, opts);
    }
}
