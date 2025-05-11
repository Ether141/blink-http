using System.Text.Json.Serialization;

namespace BlinkHttp.Swagger.Structure;

internal class Schema
{
    [JsonInclude]
    internal string Type { get; }

    public Schema(SchemaType type)
    {
        Type = type.ToString().ToLower();
    }
}
