namespace rsc_converter.Classes.Utils;

public static class StringUtils
{
    public static string Escape(this string str)
    {
        return str.Replace("\\", "\\\\")
            .Replace("\b", "\\b")
            .Replace("\t", "\\t")
            .Replace("\n", "\\n")
            .Replace("\f", "\\f")
            .Replace("\r", "\\r")
            .Replace("\0", "\\0")
            .Replace("\"", "\\\"")
            .Replace("'", "\\'");
    }
}
