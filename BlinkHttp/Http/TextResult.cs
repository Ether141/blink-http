#pragma warning disable CS1591

using System.Net;
using System.Text;

namespace BlinkHttp.Http;

/// <summary>
/// <seealso cref="IHttpResult"/> as text/plain response.
/// </summary>
public class TextResult : IHttpResult
{
    public byte[] Data => Encoding.UTF8.GetBytes(text);
    public string? ContentType => MimeTypes.TextPlain;
    public string? ContentDisposition => null;
    public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;

    private readonly string text;

    public TextResult(string text)
    {
        this.text = text;
    }

    public TextResult(string text, HttpStatusCode httpCode)
    {
        this.text = text;
        HttpCode = httpCode;
    }
}
