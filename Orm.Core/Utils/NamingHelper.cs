using System.Text;

namespace Orm.Core.Utils;
public static class NamingHelper
{
    public static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (char.IsUpper(c))
            {
                if (i > 0)
                {
                    stringBuilder.Append('_');
                }
                stringBuilder.Append(char.ToLower(c));
            }
            else
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString();
    }
}
