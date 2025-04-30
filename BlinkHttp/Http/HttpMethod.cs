namespace BlinkHttp.Http;

/// <summary>
/// Defines HTTP methods supported by server.
/// </summary>
public enum HttpMethod
{
#pragma warning disable CS1591
    Get,
    Post,
    Delete,
    Put
#pragma warning restore CS1591
}

internal static class HttpMethodExtension
{
    internal static HttpMethod Parse(string method) => method.ToLower() switch
    {
        "get" => HttpMethod.Get,
        "post" => HttpMethod.Post,
        "delete" => HttpMethod.Delete,
        "put" => HttpMethod.Put,
        _ => throw new ArgumentException($"{method} cannot be parsed as any HTTP method from HttpMethod enum.")
    };
}
