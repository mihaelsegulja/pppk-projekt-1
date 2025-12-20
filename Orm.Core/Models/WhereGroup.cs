namespace Orm.Core.Models;

public class WhereGroup
{
    public SqlLogicalOperatorType Operator { get; set; } = SqlLogicalOperatorType.And;
    public List<WhereCondition> Conditions { get; } = new();
}


public enum SqlLogicalOperatorType
{
    And,
    Or,
}