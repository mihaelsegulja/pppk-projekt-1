namespace Orm.Core.Mapping;

public class TypeMapper
{
    public static string ToDbType(Type type) => type switch
    {
        _ when type == typeof(int) => "INTEGER",
        _ when type == typeof(long) => "BIGINT",
        _ when type == typeof(decimal) => "DECIMAL",
        _ when type == typeof(float) => "REAL",
        _ when type == typeof(double) => "DOUBLE PRECISION",
        _ when type == typeof(char) => "CHAR(1)",
        _ when type == typeof(string) => "TEXT",
        _ when type == typeof(DateTime) => "TIMESTAMP WITHOUT TIME ZONE",
        _ when type == typeof(DateTimeOffset) => "TIMESTAMP WITH TIME ZONE",
        _ when type == typeof(Guid) => "UUID",
        _ => throw new NotSupportedException($"Unsupported .NET type: {type}")
    };
}