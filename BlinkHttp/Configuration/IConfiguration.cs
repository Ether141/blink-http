namespace BlinkHttp.Configuration;

/// <summary>
/// Handles application configuration loading, processing and obtaining values from it.
/// </summary>
public interface IConfiguration
{
    /// <summary>
    /// Returns value from configuration with given key. If key does not exist, returns null.
    /// </summary>
    string? this[string key] { get; }

    /// <summary>
    /// Returns value from configuration with given index. If value with given index does not exist, returns null.
    /// </summary>
    string? this[int index] { get; }


    /// <summary>
    /// Returns value from configuration with given key and casts it to the given type. If key does not exist, throws exception.
    /// </summary>
    T Get<T>(string key);

    /// <summary>
    /// Returns value from configuration with given key as string. If key does not exist, throws exception.
    /// </summary>
    string Get(string key);

    /// <summary>
    /// Returns value from configuration with given key as array of values. If key does not exist, throws exception.
    /// </summary>
    string[] GetArray(string key);

    /// <summary>
    /// Returns value from configuration with given key as array of values casted to given type. If key does not exist, throws exception.
    /// </summary>
    T[] GetArray<T>(string key);
}
