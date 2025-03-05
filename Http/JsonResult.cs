using System.Text;
using System.Text.Json;

namespace BlinkHttp.Http
{
    internal class JsonResult : IHttpResult
    {
        private readonly string jsonValue;

        public byte[] Data => Encoding.UTF8.GetBytes(jsonValue);
        public string ContentType => MimeTypes.ApplicationJson;

        public JsonResult(string jsonValue)
        {
            this.jsonValue = jsonValue;
        }

        internal static JsonResult FromObject(object? obj) => new JsonResult(JsonSerializer.Serialize(obj, JsonSerializerOptions.Web));
    }
}
