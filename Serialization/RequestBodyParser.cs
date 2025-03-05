using BlinkHttp.Http;

namespace BlinkHttp.Serialization;

internal static class RequestBodyParser
{
    internal static void ParseBody(RequestContent content)
    {
        string[] split = content.ContentType.Split(';');
        string mimeType = split.Length == 0 ? content.ContentType : split[0];

        IDataParser parser = GetParser(mimeType);
        parser.Parse(content);
    }

    private static IDataParser GetParser(string mimeType)
        => mimeType switch
        {
            MimeTypes.MultipartFormData => new FormDataParser(),
            _ => throw new NotSupportedException()
        };
}
