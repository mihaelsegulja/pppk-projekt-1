namespace Orm.Core.Utils;

internal static class ToSqlHelper
{
    public static string FormatValue(object value)
    {
        return value switch
        {
            null => "NULL",
            string s => $"'{s.Replace("'", "''")}'",
            char c => $"'{c}'",
            bool b => b ? "TRUE" : "FALSE",
            DateTime dt => $"'{dt:yyyy-MM-dd HH:mm:ss}'",
            DateTimeOffset dto => $"'{dto:yyyy-MM-dd HH:mm:sszzz}'",
            _ => value.ToString() ?? "NULL",
        };
    }

}
