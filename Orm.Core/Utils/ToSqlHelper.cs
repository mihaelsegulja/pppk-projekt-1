namespace Orm.Core.Utils;

internal static class ToSqlHelper
{
    public static string FormatDefaultValue(object defaultValue)
    {
        return defaultValue switch
        {
            string str => $"'{str.Replace("'", "''")}'",
            bool b => b ? "1" : "0",
            null => "NULL",
            _ => defaultValue.ToString() ?? "NULL",
        };
    }
}
