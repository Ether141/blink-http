using System.Text;

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
    }
}
