namespace Orm.Core.Models;

public class OrderByCondition
{
    public string ColumnName { get; set; }
    public OrderByType OrderBy { get; set; } = OrderByType.Asc;
}

public enum OrderByType
{
    Asc,
    Desc
}