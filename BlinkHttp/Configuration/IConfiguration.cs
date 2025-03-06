namespace BlinkHttp.Configuration;

public interface IConfiguration
{
    string? this[string key] { get; }
    string? this[int index] { get; }

    T Get<T>(string key);
    string Get(string key);
    string[] GetArray(string key);
    T[] GetArray<T>(string key);
}
