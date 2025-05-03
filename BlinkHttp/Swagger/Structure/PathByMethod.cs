using System.Text.Json.Serialization;

namespace BlinkHttp.Swagger.Structure;

internal class PathByMethod
{
    [JsonInclude]
    internal string? Summary { get; init; }

    [JsonInclude]
    internal string[]? Tags { get; init; }

    [JsonInclude]
    internal Dictionary<string, Response>? Responses { get; init; }

    [JsonInclude]
    internal Parameter[]? Parameters { get; init; }
}
