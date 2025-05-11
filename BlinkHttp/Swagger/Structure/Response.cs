using System.Text.Json.Serialization;

namespace BlinkHttp.Swagger.Structure;

internal class Response
{
    [JsonInclude]
    internal string Description { get; }

    [JsonInclude]
    internal Dictionary<string, Schema>? Content { get; init; }

    public Response(string description)
    {
        Description = description;
    }

    public Response(ResponseMetadata metadata)
    {
        Description = metadata.Description;
        
        if (metadata.MimeType != null)
        {
            Content = new() { { metadata.MimeType, new Schema(metadata.SchemaType) } };
        }
    }
}
