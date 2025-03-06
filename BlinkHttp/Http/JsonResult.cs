using System.Text;
using System.Text.Json;

namespace BlinkHttp.Http;

public class JsonResult : IHttpResult
{
    private readonly string jsonValue;

    public byte[] Data => Encoding.UTF8.GetBytes(jsonValue);
    public string ContentType => MimeTypes.ApplicationJson;
    public string? ContentDisposition => null;

    public JsonResult(string jsonValue)
    {
        this.jsonValue = jsonValue;
    }

    public static JsonResult FromObject(object? obj) => new JsonResult(JsonSerializer.Serialize(obj, JsonSerializerOptions.Web));
}
