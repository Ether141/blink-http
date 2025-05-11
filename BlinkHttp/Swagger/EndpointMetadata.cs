using BlinkHttp.Http;
using System.Net;

namespace BlinkHttp.Swagger;

/// <summary>
/// Represents metadata for an API endpoint, used for generating Swagger documentation.
/// </summary>
public class EndpointMetadata
{
    /// <summary>
    /// Gets the path of the API endpoint.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the HTTP method (e.g., GET, POST) of the API endpoint.
    /// </summary>
    public Http.HttpMethod HttpMethod { get; }

    /// <summary>
    /// Gets or sets a brief summary of the API endpoint.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the response descriptions for the API endpoint, 
    /// where the key is the HTTP status code and the value is the description.
    /// </summary>
    public Dictionary<HttpStatusCode, ResponseMetadata>? Responses { get; set; }

    /// <summary>
    /// Gets or sets the descriptions of the parameters for the API endpoint, 
    /// where the key is the parameter name and the value is the description.
    /// </summary>
    public Dictionary<string, string>? ParameterDescriptions { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EndpointMetadata"/> class.
    /// </summary>
    /// <param name="path">The path of the API endpoint.</param>
    /// <param name="httpMethod">The HTTP method of the API endpoint.</param>
    public EndpointMetadata(string path, Http.HttpMethod httpMethod)
    {
        Path = path;
        HttpMethod = httpMethod;
    }
}
