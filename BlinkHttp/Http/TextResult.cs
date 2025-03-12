using System.Text;

namespace BlinkHttp.Http;

public class TextResult : IHttpResult
{
    public byte[] Data => Encoding.UTF8.GetBytes(text);
    public string ContentType => MimeTypes.TextPlain;
    public string? ContentDisposition => null;

    private readonly string text;

    public TextResult(string text)
    {
        this.text = text;
    }
}
