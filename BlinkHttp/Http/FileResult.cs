using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// Represents a result containing file data to be returned in an HTTP response.
/// Implements the <see cref="IHttpResult"/> interface.
/// </summary>
public class FileResult : IHttpResult
{
    /// <summary>
    /// Gets the file data as a byte array.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Gets the MIME type of the file.
    /// </summary>
    public string? ContentType { get; }

    /// <summary>
    /// Gets the Content-Disposition header value, which specifies how the file should be presented (e.g., inline or attachment).
    /// </summary>
    public string? ContentDisposition { get; }

    /// <summary>
    /// Gets or sets the HTTP status code for the response.
    /// Defaults to <see cref="HttpStatusCode.OK"/>.
    /// </summary>
    public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileResult"/> class with "inline" Content-Disposition.
    /// </summary>
    private FileResult(byte[] data, string contentType)
    {
        Data = data;
        ContentType = contentType;
        ContentDisposition = "inline";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileResult"/> class with "attachment" Content-Disposition.
    /// </summary>
    private FileResult(byte[] data, string contentType, string? fileName)
    {
        Data = data;
        ContentType = contentType;
        ContentDisposition = "attachment" + ($"; filename=\"{fileName}\"" ?? "");
    }

    /// <summary>
    /// Creates a new instance of <see cref="FileResult"/> with "inline" Content-Disposition.
    /// </summary>
    public static FileResult Inline(byte[] data, string contentType) => new FileResult(data, contentType);

    /// <summary>
    /// Creates a new instance of <see cref="FileResult"/> with "attachment" Content-Disposition.
    /// </summary>
    public static FileResult Attachment(byte[] data, string contentType) => new FileResult(data, contentType, null);

    /// <summary>
    /// Creates a new instance of <see cref="FileResult"/> with "attachment" Content-Disposition and a specified file name.
    /// </summary>
    public static FileResult Attachment(byte[] data, string contentType, string fileName) => new FileResult(data, contentType, fileName);
}
