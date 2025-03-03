using BlinkHttp.Http;
using BlinkHttp.Routing;

namespace BlinkHttp
{
    [Route("user")]
    internal class UserController : Controller
    {
        [HttpGet("get/{id}")]
        public IHttpResult Get()
        {
            return new JsonResult("""{ "value": "something" }""");
        }
    }

    [Route("[controller]")]
    internal class BasketController : Controller
    {

    }

    internal class ProductController : Controller
    {

    }
}
