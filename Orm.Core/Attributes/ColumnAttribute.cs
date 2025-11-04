using Orm.Core.Utils;

namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public string Name { get; }

    public ColumnAttribute()
    {
        Name = NamingHelper.ToSnakeCase(GetType().Name);
    }

    public ColumnAttribute(string? name)
    {
        Name = name;
    }
}
