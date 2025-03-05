using BlinkHttp.Http;

namespace BlinkHttp.Routing;

internal abstract class Controller
{
    internal HttpContext Context { get; set; }
}
