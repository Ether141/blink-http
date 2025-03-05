using System.Text;

namespace BlinkHttp.Serialization;

internal class FormDataParser : IDataParser
{
    private RequestContent? currentContent;

    public void Parse(RequestContent content)
    {
        currentContent = content;

        string? boundary = GetBoundary(content.ContentType) ?? throw new RequestBodyInvalidException("Unable to read request body content - boundary is missing.");

        //Console.WriteLine(content.ReadToEnd());
        Console.WriteLine(boundary);

        ParseFormData(boundary);
    }

    private void ParseFormData(string boundary)
    {
        using Stream stream = currentContent!.Stream;
        using StreamReader reader = new StreamReader(stream, currentContent.Encoding);

        int valuesCount = 0;
        string? currentName;
        string? currentFileName;

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine()!;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line == $"--{boundary}" && !reader.EndOfStream)
            {
                currentName = currentFileName = null;
                valuesCount++;
                continue;
            }

            if (line.StartsWith("Content-Disposition:"))
            {
                string[] split = line.Split(';', StringSplitOptions.TrimEntries);
                currentName = split.FirstOrDefault(s => s.StartsWith("name="))?[6..^1];
                currentFileName = split.FirstOrDefault(s => s.StartsWith("filename="))?[10..^1];
                Console.WriteLine($"{currentName} | {currentFileName}");
                continue;
            }
        }

        Console.WriteLine(valuesCount);
    }

    private static string? GetBoundary(string contentType)
    {
        string[] split = contentType.Split(';');
        string? boundary = split.FirstOrDefault(s => s.Contains("boundary="))?.Trim();
        return boundary?[9..];
    }
}
