namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public string? Name { get; set; }
}
