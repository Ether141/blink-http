using System.Text.Json.Serialization;

namespace BlinkHttp.Swagger.Structure;

internal class Info
{
    [JsonInclude]
    internal string? Title { get; init; }

    [JsonInclude]
    internal string? Version { get; init; }
}
