using BlinkHttp.Http;
using BlinkHttp.Routing;
using BlinkHttp.Serialization;

namespace BlinkHttp
{
    [Route("user")]
    internal class UserController : Controller
    {
        [HttpGet("{id}/get/{value}")]
        public IHttpResult Get(int id, string value, [Optional] string? author)
        {
            return JsonResult.FromObject(new { id, value, author });
        }
    }

    [Route("[controller]")]
    internal class BasketController : Controller
    {
        public IHttpResult Get()
        {
            return new JsonResult("""{ "value": "basket1" }""");
        }
    }
}
