namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class DefaultAttribute : Attribute
{
    public object Value { get; set; }

    public DefaultAttribute(object value)
    {
        Value = value;
    }
}
