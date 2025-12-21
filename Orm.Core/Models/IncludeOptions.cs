namespace Orm.Core.Models;

public class IncludeOptions
{
    internal List<Type> IncludedTypes { get; } = new();

    public void Include<T>()
    {
        IncludedTypes.Add(typeof(T));
    }
}