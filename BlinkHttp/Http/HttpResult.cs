using System.Net;

namespace BlinkHttp.Http;

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
