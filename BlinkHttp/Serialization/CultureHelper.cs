using System.Globalization;

namespace BlinkHttp.Serialization;

internal static class CultureHelper
{
    private static readonly CultureInfo enCulture;

    internal static CultureInfo DefaultCulture => enCulture;
    internal static NumberFormatInfo DefaultNumberFormat => enCulture.NumberFormat;

    static CultureHelper()
    {
        enCulture = new CultureInfo("en-US");
    }
}
