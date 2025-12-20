namespace Orm.Core.Models;

public class DeleteOptions
{
    public List<WhereGroup> WhereGroups { get; } = new();
}