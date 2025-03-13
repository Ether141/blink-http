using BlinkHttp.Http;

namespace BlinkHttp.Serialization;

internal class RequestValue
{
    public string Name { get; }
    public string[]? Values { get; }
    internal RequestFile? File { get; }
    public string? InternalObjectName { get; set; }

    internal bool IsFile => File != null;
    internal bool IsInternalObject => InternalObjectName != null;

    public RequestValue(string name, string[] values)
    {
        Name = name;
        Values = values;
    }

    public RequestValue(string name, string fileName, byte[] data)
    {
        Name = name;
        File = new RequestFile(fileName, data);
    }
}
