namespace Orm.Core.Models;

internal class EntitySnapshot
{
    public Dictionary<string, object> Values { get; set; } = new();
}