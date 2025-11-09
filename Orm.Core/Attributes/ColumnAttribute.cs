using Orm.Core.Utils;

namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public string? Name { get; }

    public ColumnAttribute(string? name = null)
    {
        Name = name;
    }
}
