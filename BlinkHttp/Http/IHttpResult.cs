namespace BlinkHttp.Http;

public interface IHttpResult
{
    public byte[] Data { get; }
    public string ContentType { get; }
    public string? ContentDisposition { get; }
}
