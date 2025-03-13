using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// <seealso cref="IHttpResult"/> as informations about file that will be returned to the user in the response.
/// </summary>
public class FileResult : IHttpResult
{
    public byte[] Data { get; }
    public string ContentType { get; }
    public string? ContentDisposition { get; }
    public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;

    private FileResult(byte[] data, string contentType)
    {
        Data = data;
        ContentType = contentType;
        ContentDisposition = "inline";
    }

    private FileResult(byte[] data, string contentType, string? fileName)
    {
        Data = data;
        ContentType = contentType;
        ContentDisposition = "attachment" + ($"; filename=\"{fileName}\"" ?? "");
    }

    /// <summary>
    /// Create new instance of <seealso cref="FileResult"/> with "inline" Content-Disposition.
    /// </summary>
    public static FileResult Inline(byte[] data, string contentType) => new FileResult(data, contentType);

    /// <summary>
    /// Create new instance of <seealso cref="FileResult"/> with "attachment" Content-Disposition.
    /// </summary>
    public static FileResult Attachment(byte[] data, string contentType) => new FileResult(data, contentType, null);

    /// <summary>
    /// Create new instance of <seealso cref="FileResult"/> with "attachment" Content-Disposition.
    /// </summary>
    public static FileResult Attachment(byte[] data, string contentType, string fileName) => new FileResult(data, contentType, fileName);
}
