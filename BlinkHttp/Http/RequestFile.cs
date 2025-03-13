namespace BlinkHttp.Http;

/// <summary>
/// Encapsulates file from request body - its binary data and filename from Content-Disposition header.
/// </summary>
public class RequestFile
{
    public string FileName { get; }
    public byte[] Data { get; }

    public RequestFile(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
