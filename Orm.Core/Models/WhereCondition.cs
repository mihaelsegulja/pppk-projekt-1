namespace Orm.Core.Models;

public class WhereCondition
{
    public WhereCondition(string columnName, SqlConditionOperatorType op, object value)
    {
        ColumnName = columnName;
        Operator = op;
        Value = value;
    }

    public string ColumnName { get; set; }
    public SqlConditionOperatorType Operator { get; set; }
    public object Value { get; set; }
}

public enum SqlConditionOperatorType
{
    Eq,
    Neq,
    Gt,
    Gte,
    Lt,
    Lte,
    Like,
    NotLike,
    In,
    NotIn,
}
