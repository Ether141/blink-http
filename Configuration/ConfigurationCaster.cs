namespace BlinkHttp.Configuration;

internal static class ConfigurationCaster
{
    internal static T[] ArrayToType<T>(string[] vals)
    {
        T[] values = new T[vals.Length];

        for (int i = 0; i < vals.Length; i++)
        {
            try
            {
                values[i] = (T)Convert.ChangeType(vals[i], typeof(T));
            }
            catch
            {
                throw new ApplicationConfigurationException($"Unable to convert values in array to the same type '{typeof(T)}'");
            }
        }

        return values;
    }

    internal static T ToType<T>(string value)
    {
        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            throw new InvalidCastException($"Unable to convert value '{value}' to type '{typeof(T)}'.");
        }
    }
}
