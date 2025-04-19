using System.Net;

namespace BlinkHttp.Handling;

internal class MiddlewareHandler
{
    private readonly IEnumerable<IMiddleware> middlewares;

    internal MiddlewareHandler(IEnumerable<IMiddleware> middlewares)
    {
        this.middlewares = middlewares;
    }

    internal bool Handle(HttpListenerRequest request,  HttpListenerResponse response)
    {
        if (!middlewares.Any())
        {
            return true;
        }

        foreach (IMiddleware middleware in middlewares)
        {
            if (!middleware.Handle(request, response))
            {
                return false;
            }
        }

        return true;
    }
}
