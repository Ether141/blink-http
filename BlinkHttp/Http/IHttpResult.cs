using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// Interface for an object that will be returned from every API endpoint.
/// </summary>
public interface IHttpResult
{
    byte[] Data { get; }
    string? ContentType { get; }
    string? ContentDisposition { get; }
    HttpStatusCode HttpCode { get; set; }
}
