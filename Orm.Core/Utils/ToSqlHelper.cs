using Orm.Core.Models;

namespace Orm.Core.Utils;

internal static class ToSqlHelper
{
    public static string ToDbType(Type type) => type switch
    {
        _ when type == typeof(int) => "INTEGER",
        _ when type == typeof(long) => "BIGINT",
        _ when type == typeof(decimal) => "DECIMAL",
        _ when type == typeof(float) => "REAL",
        _ when type == typeof(double) => "DOUBLE PRECISION",
        _ when type == typeof(char) => "CHAR(1)",
        _ when type == typeof(string) => "TEXT",
        _ when type == typeof(DateTime) => "TIMESTAMP WITHOUT TIME ZONE",
        _ when type == typeof(DateTimeOffset) => "TIMESTAMP WITH TIME ZONE",
        _ when type == typeof(Guid) => "UUID",
        _ => throw new NotSupportedException($"Unsupported type: {type}")
    };
    
    public static string FormatValue(object? value) => value switch
    {
        null => "NULL",
        string s => $"'{s.Replace("'", "''")}'",
        char c => $"'{c}'",
        bool b => b ? "TRUE" : "FALSE",
        DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
        DateTimeOffset dto => $"'{dto:yyyy-MM-dd HH:mm:sszzz}'",
        IEnumerable<object> ie => $"({string.Join(", ", ie.Select(FormatValue))})",
        _ => value.ToString()!
    };

    public static string ToSqlConditionalOperator(SqlConditionOperatorType op) => op switch
    {
        SqlConditionOperatorType.Eq => "=",
        SqlConditionOperatorType.Neq => "<>",
        SqlConditionOperatorType.Gt => ">",
        SqlConditionOperatorType.Gte => ">=",
        SqlConditionOperatorType.Lt => "<",
        SqlConditionOperatorType.Lte => "<=",
        SqlConditionOperatorType.Like => "LIKE",
        SqlConditionOperatorType.NotLike => "NOT LIKE",
        SqlConditionOperatorType.In => "IN",
        SqlConditionOperatorType.NotIn => "NOT IN",
        _ => throw new NotSupportedException($"Unsupported conditional operator: {op}")
    };
    
    public static string ToSqlOrder(OrderByType order) =>
        order == OrderByType.Asc ? "ASC" : "DESC";

    public static string ToSqlLogicalOperator(SqlLogicalOperatorType op) => op switch
    {
        SqlLogicalOperatorType.And => "AND",
        SqlLogicalOperatorType.Or => "OR",
        _ => throw new NotSupportedException($"Unsupported logical operator: {op}")
    };
}
