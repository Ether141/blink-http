using System.Net;

namespace BlinkHttp.Swagger;

/// <summary>
/// Represents metadata for an HTTP response in Swagger documentation.
/// </summary>
public class ResponseMetadata
{
    /// <summary>
    /// Gets the description of the response.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets the MIME type of the response.
    /// </summary>
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the schema type of the response.
    /// </summary>
    public SchemaType SchemaType { get; set; } = SchemaType.Object;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseMetadata"/> class with the specified description.
    /// </summary>
    /// <param name="description">The description of the response.</param>
    public ResponseMetadata(string description)
    {
        Description = description;
    }
}
