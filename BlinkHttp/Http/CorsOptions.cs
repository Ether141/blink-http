namespace BlinkHttp.Http;

/// <summary>
/// Represents the configuration settings for Cross-Origin Resource Sharing (CORS).
/// </summary>
public class CorsOptions
{
    /// <summary>
    /// Gets or sets the allowed origin for CORS requests.
    /// Default value is "*" which permits all origins.
    /// </summary>
    public string Origin { get; set; } = "*";

    /// <summary>
    /// Gets or sets the allowed headers in CORS requests.
    /// Default value is "*" which permits all headers.
    /// </summary>
    public string Headers { get; set; } = "*";
}
