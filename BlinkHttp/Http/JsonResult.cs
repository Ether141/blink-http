using System.Net;
using System.Text;
using System.Text.Json;

namespace BlinkHttp.Http;

/// <summary>
/// <seealso cref="IHttpResult"/> as JSON object.
/// </summary>
public class JsonResult : IHttpResult
{
    private readonly string jsonValue;

    public byte[] Data => Encoding.UTF8.GetBytes(jsonValue);
    public string ContentType => MimeTypes.ApplicationJson;
    public string? ContentDisposition => null;
    public HttpStatusCode HttpCode { get; set; } = HttpStatusCode.OK;

    public JsonResult(string jsonValue)
    {
        this.jsonValue = jsonValue;
    }

    public JsonResult(string jsonValue, HttpStatusCode httpCode)
    {
        this.jsonValue = jsonValue;
        HttpCode = httpCode;
    }

    /// <summary>
    /// Serializes a given <seealso cref="object"/> and initializes new instance of <seealso cref="JsonResult"/> with this serialized object.
    /// </summary>
    public static JsonResult FromObject(object? obj) => new JsonResult(JsonSerializer.Serialize(obj, JsonSerializerOptions.Web));

    /// <summary>
    /// Serializes a given <seealso cref="object"/> and initializes new instance of <seealso cref="JsonResult"/> with this serialized object and given <seealso cref="HttpStatusCode"/>.
    /// </summary>
    public static JsonResult FromObject(object? obj, HttpStatusCode httpCode) => new JsonResult(JsonSerializer.Serialize(obj, JsonSerializerOptions.Web), httpCode);
}
