using System.Globalization;

namespace BlinkHttp.Logging;

internal static class LoggingHelper
{
    internal const string ShortDateFormat = "%d";
    internal const string LongDateFormat = "%D";
    internal const string ShortTimeOnlyFormat = "%t";
    internal const string LongTimeOnlyFormat = "%T";

    internal const string LoggerNameFormat = "%name";
    internal const string MessageFormat = "%message";
    internal const string LogLevelFormat = "%level";

    internal const string DefaultFormat = "%D | %level:-5 | [%name] | %message";

    internal static string GetFormattedString(string message, string? loggerName, LogLevel logLevel, string? format)
    {
        loggerName ??= " ";

        string formatted = format ?? DefaultFormat;

        formatted = formatted.Replace(ShortDateFormat, GetDate(ShortDateFormat))
                             .Replace(LongDateFormat, GetDate(LongDateFormat))
                             .Replace(ShortTimeOnlyFormat, GetDate(ShortTimeOnlyFormat))
                             .Replace(LongTimeOnlyFormat, GetDate(LongTimeOnlyFormat));

        int padding = GetPadding(LoggerNameFormat, ref formatted);
        formatted = formatted.Replace(LoggerNameFormat, Pad(loggerName, padding));

        padding = GetPadding(LogLevelFormat, ref formatted);
        formatted = formatted.Replace(LogLevelFormat, Pad(logLevel.GetString(), padding));

        if (formatted.Contains(MessageFormat))
        {
            formatted = formatted.Replace(MessageFormat, message);
        }
        else
        {
            formatted += $" {message}";
        }

        return formatted;
    }

    private static string GetDate(string format) => format switch
    {
        ShortDateFormat => DateTime.Now.ToString("MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
        LongDateFormat => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
        ShortTimeOnlyFormat => DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
        LongTimeOnlyFormat => DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture),
        _ => throw new ArgumentException()
    };

    private static int GetPadding(string format, ref string formatted)
    {
        int len = format.Length;
        int pad = 0;

        if (formatted.Contains(format + ":"))
        {
            int index = formatted.IndexOf(format + ":");
            string padString = string.Concat(formatted.Skip(index + len + 1).TakeWhile((c, index) => char.IsNumber(c) || index == 0 && c == '-'));

            if (padString != "")
            {
                pad = int.Parse(padString);
                formatted = formatted[..(index + len)] + formatted[(index + len + 1 + padString.Length)..];
            }
        }

        return pad;
    }

    private static string Pad(string text, int pad) => pad > 0 ? text.PadLeft(pad) : text.PadRight(pad *= -1);
}
