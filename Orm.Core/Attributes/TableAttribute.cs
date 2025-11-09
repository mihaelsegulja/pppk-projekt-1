using Orm.Core.Utils;

namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    public string? Name { get; }

    public TableAttribute(string? name = null)
    {
        Name = name;
    }
}
