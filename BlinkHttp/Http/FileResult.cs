namespace BlinkHttp.Http;

public class FileResult : IHttpResult
{
    public byte[] Data { get; }
    public string ContentType { get; }
    public string? ContentDisposition { get; }

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

    public static FileResult Inline(byte[] data, string contentType) => new FileResult(data, contentType);

    public static FileResult Attachment(byte[] data, string contentType) => new FileResult(data, contentType, null);

    public static FileResult Attachment(byte[] data, string contentType, string fileName) => new FileResult(data, contentType, fileName);
}
