namespace BlinkHttp.Http;

/// <summary>
/// Encapsulates file from request body - its binary data and filename from Content-Disposition header.
/// </summary>
public class RequestFile
{
    /// <summary>
    /// Name of the file.
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// File content as raw, binary data.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Creates new instance of <seealso cref="RequestFile"/> with specified file name and file content.
    /// </summary>
    public RequestFile(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}
