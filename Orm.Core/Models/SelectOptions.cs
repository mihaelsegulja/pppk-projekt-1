namespace Orm.Core.Models;

public class SelectOptions
{
    public List<WhereGroup> WhereGroups { get; } = new();
    public List<OrderByCondition> OrderBy { get; } = new();
    public int? Limit { get; set; }
    public int? Offset { get; set; }
}