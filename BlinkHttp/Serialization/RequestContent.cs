using System.Text;

namespace BlinkHttp.Serialization;

internal sealed class RequestContent
{
    internal string ContentType { get; }
    internal long ContentLength { get; }
    internal Stream Stream { get; }
    internal Encoding Encoding { get; }

    internal RequestContent(string contentType, long contentLength, Stream stream, Encoding encoding)
    {
        ContentType = contentType;
        ContentLength = contentLength;
        Stream = stream;
        Encoding = encoding;
    }

    internal string? ReadToEnd()
    {
        try
        {
            using Stream body = Stream;
            using StreamReader reader = new StreamReader(body, Encoding);
            return reader.ReadToEnd();
        }
        catch
        {
            return null;
        }
    }
}
