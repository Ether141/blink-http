using BlinkHttp.Http;
using BlinkHttp.Routing;
using BlinkHttp.Swagger.Structure;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlinkHttp.Swagger;

internal class OpenApiGenerator
{
    private readonly string title;
    private readonly string version;

    private readonly EndpointMetadata[] metadata;

    internal string? Json { get; private set; }

    public OpenApiGenerator(string title, string version, EndpointMetadata[] metadata)
    {
        this.title = title;
        this.version = version;
        this.metadata = metadata;
    }

    internal void GenerateJson(IReadOnlyCollection<IRoutesCollection> routes)
    {
        Dictionary<string, Dictionary<string, PathByMethod>> paths = [];

        foreach (IRoutesCollection collection in routes)
        {
            string tag = "/" + collection.ControllerPath;
            var grouped = collection.Routes.GroupBy(r => $"{tag}/{r.Path}");

            foreach (var group in grouped)
            {
                Dictionary<string, PathByMethod> dict = [];

                foreach (var route in group)
                {
                    string path = $"{tag}/{route.Path}";
                    if (path.EndsWith('/')) path = path[..^1];

                    EndpointMetadata? endpointMetadata = metadata.FirstOrDefault(m => m.Path == path && m.HttpMethod.ToString().Equals(route.HttpMethod.ToString(), StringComparison.OrdinalIgnoreCase));
                    string? summary = endpointMetadata?.Summary;

                    PathByMethod pathByMethod = new PathByMethod
                    {
                        Tags = [tag[1..]],
                        Summary = summary,
                        Responses = MetadataToResponses(endpointMetadata),
                        Parameters = GetUrlParameters(route.Endpoint.MethodInfo, endpointMetadata)
                    };

                    dict.Add(route.HttpMethod.ToString().ToLower(), pathByMethod);
                }

                paths.Add(group.Key, dict);
            }
        }

        Docs docs = new Docs
        {
            Openapi = "3.0.0",
            Info = new Info
            {
                Title = title,
                Version = version
            },
            Paths = paths
        };

        JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        Json = JsonSerializer.Serialize(docs, options);
    }

    private static Parameter[] GetUrlParameters(MethodInfo methodInfo, EndpointMetadata? metadata)
    {
        ParameterInfo[] parameterInfos = methodInfo.GetParameters();
        List<Parameter> parameters = new List<Parameter>();

        foreach (ParameterInfo parameter in parameterInfos)
        {
            IEnumerable<Attribute> attributes = parameter.GetCustomAttributes();

            if (!attributes.Any(a => a.GetType() == typeof(FromQueryAttribute)))
            {
                continue;
            }

            Type parameterType = parameter.ParameterType;

            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                parameterType = Nullable.GetUnderlyingType(parameterType)!;
            }

            SchemaType schemaType = parameterType switch
            {
                Type t when t == typeof(string) => SchemaType.String,
                Type t when t == typeof(int) => SchemaType.Integer,
                Type t when t == typeof(bool) => SchemaType.Boolean,
                Type t when t == typeof(double) => SchemaType.Number,
                Type t when t == typeof(decimal) => SchemaType.Number,
                Type t when t == typeof(float) => SchemaType.Number,
                Type t when t.IsArray => SchemaType.Array,
                _ => SchemaType.Object
            };

            string? description = metadata?.ParameterDescriptions?.FirstOrDefault(d => d.Key.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase)).Value;

            Parameter para = new Parameter
            {
                Name = parameter.Name,
                Required = attributes.All(a => a.GetType() != typeof(OptionalAttribute)),
                Schema = new Schema(schemaType),
                Description = description
            };

            parameters.Add(para);
        }

        return [.. parameters];
    }

    private static Dictionary<string, Response>? MetadataToResponses(EndpointMetadata? metadata)
    {
        if (metadata == null || metadata.Responses == null)
        {
            return null;
        }

        Dictionary<string, Response> dict = [];

        foreach (var response in metadata.Responses)
        {
            dict.Add(((int)response.Key).ToString(), new Response(response.Value));
        }

        return dict;
    }
}
