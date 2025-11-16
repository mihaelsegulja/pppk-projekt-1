using System.Reflection;

namespace Orm.Core.Models;

public record ColumnMetadata
{
    public PropertyInfo Property { get; set; }
    public string ColumnName { get; set; }
    public Type RuntimeType { get; set; }
    public string DbType { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsAutoIncrement { get; set; }
    public bool IsNotNull { get; set; }
    public bool IsUnique { get; set; }
    public object? DefaultValue { get; set; }
}