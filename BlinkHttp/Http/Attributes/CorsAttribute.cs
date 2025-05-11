namespace BlinkHttp.Http;

/// <summary>
/// An attribute to define Cross-Origin Resource Sharing (CORS) settings for an HTTP endpoint.
/// </summary>
/// /// <remarks>
/// Use this attribute if you did not enable global CORS when building the WebApplication, or you want to use different options for the selected endpoint.
/// </remarks>
[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class CorsAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the allowed origin for CORS requests.
    /// If not set, defaults to "*", which permits all origins.
    /// </summary>
    public string? AllowedOrigin { get; set; }

    /// <summary>
    /// Gets or sets the allowed headers in CORS requests.
    /// If not set or empty, defaults to "*", which permits all headers.
    /// </summary>
    public string? AllowedHeaders { get; set; }

    /// <summary>
    /// Gets or sets the allowed HTTP methods in CORS requests.
    /// If not set or empty, defaults to "*", which permits all methods.
    /// </summary>
    public string? AllowedMethods { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether credentials are supported in CORS requests.
    /// Default value is <c>false</c>, which means credentials are not included.
    /// </summary>
    public bool Credentials { get; set; }
}
