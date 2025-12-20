namespace Orm.Core.Utils;

internal static class DbValueConverter
{
    public static object ConvertValue(object raw, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (underlying.IsEnum)
            return ConvertEnum(raw, underlying);

        if (underlying == typeof(Guid))
            return ConvertGuid(raw);

        if (underlying == typeof(char))
            return ConvertChar(raw);

        if (underlying == typeof(bool))
            return ConvertBool(raw);

        if (underlying == typeof(DateTimeOffset))
            return ConvertDateTimeOffset(raw);

        return Convert.ChangeType(raw, underlying);
    }

    private static object ConvertEnum(object raw, Type enumType)
    {
        if (raw is string s)
            return Enum.Parse(enumType, s);
        return Enum.ToObject(enumType, raw);
    }

    private static object ConvertGuid(object raw)
    {
        if (raw is Guid g) return g;
        return Guid.Parse(raw.ToString()!);
    }

    private static object ConvertChar(object raw)
    {
        if (raw is char c) return c;
        if (raw is string s && s.Length > 0) return s[0];
        return Convert.ChangeType(raw, typeof(char));
    }

    private static object ConvertBool(object raw)
    {
        if (raw is bool b) return b;
        return Convert.ToBoolean(raw);
    }

    private static object ConvertDateTimeOffset(object raw)
    {
        if (raw is DateTimeOffset dto) return dto;
        if (raw is DateTime dt) return new DateTimeOffset(dt);
        return DateTimeOffset.Parse(raw.ToString()!);
    }
}
