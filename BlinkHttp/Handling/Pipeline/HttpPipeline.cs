using BlinkHttp.Http;

namespace BlinkHttp.Handling.Pipeline;

internal class HttpPipeline
{
    private readonly IMiddleware[] middlewares;

    internal HttpPipeline(params IEnumerable<IMiddleware> middlewares)
    {
        this.middlewares = middlewares.ToArray();
        InitializeMiddlewares();
    }

    internal async Task Invoke(HttpContext context)
    {
        if (middlewares.Length == 0)
        {
            return;
        }

        await middlewares[0].InvokeAsync(context);
    }

    private void InitializeMiddlewares()
    {
        if (middlewares.Length == 0)
        {
            return;
        }

        for (int i = 0; i < middlewares.Length - 1; i++)
        {
            middlewares[i].Next = new MiddlewareDelegate(middlewares[i + 1].InvokeAsync);
        }

        middlewares[^1].Next = new MiddlewareDelegate(context => Task.CompletedTask);
    }
}
