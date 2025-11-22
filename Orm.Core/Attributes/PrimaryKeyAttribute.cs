namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PrimaryKeyAttribute : Attribute
{
    public bool AutoIncrement { get; set; } = false;
}
