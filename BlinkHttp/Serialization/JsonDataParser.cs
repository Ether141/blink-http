using System.Text.Json;

namespace BlinkHttp.Serialization;

internal class JsonDataParser : IDataParser
{
    public RequestValue[] Parse(RequestContent content)
    {
        string? json = content.ReadToEnd() ?? throw new RequestBodyInvalidException();
        JsonDocument jsonDocument = JsonDocument.Parse(json);
        List<RequestValue> values = [];
        FlattenElement(jsonDocument.RootElement, string.Empty, values);
        return [.. values];
    }

    private static void FlattenElement(JsonElement element, string prefix, List<RequestValue> values)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    FlattenElement(property.Value, CombinePaths(prefix, property.Name), values);
                }
                break;

            case JsonValueKind.Array:
                List<string> arrayValues = [];

                foreach (var item in element.EnumerateArray())
                {
                    arrayValues.Add(item.GetRawText());
                }

                values.Add(NewRequestValue(prefix, [.. arrayValues]));
                break;

            case JsonValueKind.String:
            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                values.Add(NewRequestValue(prefix, [element.GetRawText().Trim().Trim('"')]));
                break;

            case JsonValueKind.Null:
                values.Add(NewRequestValue(prefix, [string.Empty]));
                break;

            default:
                throw new NotSupportedException($"Unsupported JsonValueKind: {element.ValueKind}");
        }
    }

    private static RequestValue NewRequestValue(string prefix, string[] values)
    {
        if (!prefix.Contains('.'))
        {
            return new RequestValue(prefix, values);
        }

        string[] split = prefix.Split('.');
        return new RequestValue(split[^1], values) { InternalObjectName = string.Join('.', split[..^1]) };
    }

    private static string CombinePaths(string prefix, string propertyName) => string.IsNullOrEmpty(prefix) ? propertyName : $"{prefix}.{propertyName.Replace(".", "")}";
}
