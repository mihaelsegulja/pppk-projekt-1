namespace Orm.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ForeignKeyAttribute : Attribute
{
    public Type ReferencedType { get; set; }
    
    public ForeignKeyAttribute(Type referencedType)
    {
        ReferencedType = referencedType;
    }
}
