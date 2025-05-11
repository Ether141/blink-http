using BlinkHttp.Authentication;
using BlinkHttp.Http;
using BlinkHttp.Routing;

namespace BlinkHttp.Handling.Pipeline;

internal class PipelineBuilder
{
    private readonly List<IMiddleware> middlewares = [];

    internal PipelineBuilder(params IMiddleware[] middlewares)
    {
        this.middlewares.AddRange(middlewares);
    }

    internal PipelineBuilder Add(IMiddleware middleware)
    {
        if (middlewares.Contains(middleware))
        {
            throw new InvalidOperationException("This middleware is already in pipeline.");
        }

        middlewares.Add(middleware);
        return this;
    }

    internal PipelineBuilder Add(params IMiddleware[] middlewares)
    {
        foreach (IMiddleware middleware in middlewares)
        {
            Add(middleware);
        }

        return this;
    }

    internal HttpPipeline Build() => new HttpPipeline(middlewares);

    internal static PipelineBuilder GetPipelineBuilderWithDefaults(Router router, IAuthorizer? authorizer, CorsOptions? corsOptions, IMiddleware[] customMiddlewares)
    {
        PipelineBuilder builder = new PipelineBuilder(
            new ExceptionHandler(),
            new StaticFiles(),
            new Routing(router),
            new CorsHandler(corsOptions),
            new Auth(authorizer)
        );

        builder.Add(customMiddlewares);
        builder.Add(new EndpointHandler());

        return builder;
    }
}
