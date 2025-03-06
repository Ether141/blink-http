namespace BlinkHttp.Serialization;

internal class FormDataParser : IDataParser
{
    private RequestContent? currentContent;

    public RequestValue[] Parse(RequestContent content)
    {
        currentContent = content;
        string? boundary = GetBoundary(currentContent!.ContentType);

        if (string.IsNullOrEmpty(boundary))
        {
            throw new RequestBodyInvalidException("Missing boundary in content type.");
        }

        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        List<(string, string, byte[])> files = [];
        BinaryReader reader = new BinaryReader(currentContent.Stream, currentContent.Encoding, leaveOpen: true);

        byte[] boundaryBytes = currentContent.Encoding.GetBytes("--" + boundary);
        byte[] endBoundaryBytes = currentContent.Encoding.GetBytes("--" + boundary + "--");
        bool wasBoundry = false;

        while (true)
        {
            byte[] line = ReadLine(reader);
            string xxx = currentContent.Encoding.GetString(line);

            if (line == null || line.SequenceEqual(endBoundaryBytes)) break;

            if (!line.SequenceEqual(boundaryBytes) && !wasBoundry) continue;

            wasBoundry = false;

            Dictionary<string, string> headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            bool shouldBreak = false;

            while (true)
            {
                string headerLine;

                try
                {
                    headerLine = ReadLineAsString(reader);
                }
                catch
                {
                    shouldBreak = true;
                    break;
                }

                if (string.IsNullOrEmpty(headerLine)) break;

                string[] parts = headerLine.Split(':', 2);
                if (parts.Length == 2)
                {
                    headers[parts[0].Trim()] = parts[1].Trim();
                }
            }

            if (shouldBreak) break;

            if (!headers.TryGetValue("Content-Disposition", out string? contentDisposition))
            {
                continue;
            }

            Dictionary<string, string> dispositionParts = contentDisposition.Split(';')
                .Select(p => p.Trim().Split('='))
                .Where(p => p.Length == 2)
                .ToDictionary(p => p[0].Trim(), p => p[1].Trim('"'));

            if (!dispositionParts.TryGetValue("name", out string? fieldName))
            {
                continue;
            }

            if (dispositionParts.TryGetValue("filename", out string? fileName))
            {
                using MemoryStream memoryStream = new MemoryStream();
                CopyUntilBoundary(reader, memoryStream, boundaryBytes);
                files.Add((fieldName, fileName, memoryStream.ToArray()));
            }
            else
            {
                using var memStream = new MemoryStream();
                CopyUntilBoundary(reader, memStream, boundaryBytes);
                memStream.Position = 0;

                using var readerText = new StreamReader(memStream, currentContent.Encoding);
                string value = readerText.ReadToEnd().Trim();
                result.TryAdd(fieldName, new List<string>());
                result[fieldName].Add(value);
            }

            wasBoundry = true;
        }

        List<RequestValue> values = [];

        foreach (KeyValuePair<string, List<string>> pair in result)
        {
            values.Add(new RequestValue(pair.Key, [.. pair.Value]));
        }

        foreach ((string fieldName, string fileName, byte[] data) in files)
        {
            values.Add(new RequestValue(fieldName, fileName, data));
        }

        return [.. values];
    }

    private static string? GetBoundary(string contentType)
    {
        int boundaryStart = contentType.IndexOf("boundary=", StringComparison.OrdinalIgnoreCase);
        return boundaryStart == -1 ? null : contentType[(boundaryStart + "boundary=".Length)..];
    }

    private static byte[] ReadLine(BinaryReader reader)
    {
        using var memoryStream = new MemoryStream();

        while (true)
        {
            byte b = reader.ReadByte();
            if (b == '\n') break;
            if (b != '\r') memoryStream.WriteByte(b);
        }

        return memoryStream.ToArray();
    }

    private string ReadLineAsString(BinaryReader reader) => currentContent!.Encoding.GetString(ReadLine(reader));

    private static void CopyUntilBoundary(BinaryReader reader, Stream outputStream, byte[] boundaryBytes)
    {
        List<byte> buffer = [];
        int boundaryLength = boundaryBytes.Length;
        
        while (true)
        {
            buffer.Add(reader.ReadByte());

            if (buffer.Count < boundaryLength + 2)
                continue;

            if (buffer[buffer.Count - boundaryLength - 2] == '\r' &&
                buffer[buffer.Count - boundaryLength - 1] == '\n' &&
                buffer.Skip(buffer.Count - boundaryLength).Take(boundaryLength).SequenceEqual(boundaryBytes))
            {
                buffer.RemoveRange(buffer.Count - boundaryLength - 2, boundaryLength + 2);
                break;
            }
        }

        outputStream.Write([.. buffer], 0, buffer.Count);
    }
}
