using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// Interface for an object that will be returned from every API endpoint.
/// </summary>
public interface IHttpResult
{
    /// <summary>
    /// Gets the raw data of the HTTP response.
    /// </summary>
    byte[] Data { get; }

    /// <summary>
    /// Gets the content type of the HTTP response, typically a MIME type (e.g., "application/json").
    /// </summary>
    string? ContentType { get; }

    /// <summary>
    /// Gets the content disposition of the HTTP response, which can indicate if the content is inline or an attachment.
    /// </summary>
    string? ContentDisposition { get; }

    /// <summary>
    /// Gets or sets the HTTP status code of the response.
    /// </summary>
    HttpStatusCode HttpCode { get; set; }
}
