namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    public string? Name { get; set; }
}
