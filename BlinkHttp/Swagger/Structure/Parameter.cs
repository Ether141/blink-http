using System.Text.Json.Serialization;

namespace BlinkHttp.Swagger.Structure;

internal class Parameter
{
    [JsonInclude]
    internal string? Name { get; init; }

    [JsonInclude]
    internal string? In => "query";

    [JsonInclude]
    internal bool Required { get; init; }

    [JsonInclude]
    internal string? Description { get; init; }

    [JsonInclude]
    internal Schema? Schema { get; init; }
}
