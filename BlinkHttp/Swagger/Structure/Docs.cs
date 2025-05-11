using System.Text.Json.Serialization;

namespace BlinkHttp.Swagger.Structure;

internal class Docs
{
    [JsonInclude]
    internal string? Openapi { get; init; }

    [JsonInclude]
    internal Info? Info { get; init; }

    [JsonInclude]
    internal Dictionary<string, Dictionary<string, PathByMethod>>? Paths { get; init; }
}
