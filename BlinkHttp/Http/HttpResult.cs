#pragma warning disable CS1591

using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// Represents a result with plain HTTP response. ContentType and ContentDisposition are required.
/// </summary>
public class HttpResult : IHttpResult
{
    public byte[] Data { get; }

    public string? ContentType { get; }

    public string? ContentDisposition { get; }

    public HttpStatusCode HttpCode { get; set; }

    public HttpResult(byte[] data, string? contentType, string? contentDisposition, HttpStatusCode httpCode)
    {
        Data = data;
        ContentType = contentType;
        ContentDisposition = contentDisposition;
        HttpCode = httpCode;
    }
}
