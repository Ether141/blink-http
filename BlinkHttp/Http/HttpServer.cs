using BlinkDatabase.General;
using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using BlinkHttp.Handling;
using BlinkHttp.Handling.Pipeline;
using BlinkHttp.Routing;
using Logging;
using System.Net;

namespace BlinkHttp.Http;

internal class HttpServer
{
    private readonly HttpListener listener;
    private readonly Router router;
    private readonly Pipeline pipeline;
    private readonly IAuthorizer? authorizer;
    private readonly string[] prefixes;
    private readonly ILogger logger = Logger.GetLogger<HttpServer>();
    private readonly string? routePrefix;

    internal IReadOnlyList<string> Prefixes => prefixes;
    internal static string WebFolderPath => Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "web");

    private readonly CancellationTokenSource cts;

    internal HttpServer(IAuthorizer? authorizer, IConfiguration? configuration, IMiddleware[] middlewares, string? routePrefix, params string[] prefixes)
    {
        logger.Debug("Initializing HTTP server...");

        listener = new HttpListener();

        this.authorizer = authorizer;
        this.prefixes = prefixes;
        this.routePrefix = routePrefix;

        foreach (string prefix in prefixes)
        {
            listener.Prefixes.Add(prefix);
        }

        logger.Debug($"Prefixes: {string.Join(", ", prefixes)}");

        cts = new CancellationTokenSource();

        router = ConfigureRouter();
        pipeline = PipelineBuilder.GetPipelineBuilderWithDefaults(router, authorizer, middlewares).Build();
    }

    private Router ConfigureRouter()
    {
        Router router = new Router();
        router.Options.RoutePrefix = routePrefix;
        router.InitializeAllRoutes();
        return router;
    }

    internal async Task StartAsync()
    {
        listener.Start();
        logger.Debug("HTTP server is up and running.");

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                Task<HttpListenerContext> contextTask = listener.GetContextAsync();
                Task completedTask = await Task.WhenAny(contextTask, Task.Delay(-1, cts.Token));

                if (completedTask == contextTask)
                {
                    HttpListenerContext context = contextTask.Result;
                    _ = Task.Run(() => HandleRequestAsync(context));
                }
                else
                {
                    break;
                }
            }
        }
        catch (HttpListenerException) when (cts.Token.IsCancellationRequested)
        {
            // OK — listener has stopped
        }
        finally
        {
            listener.Stop();
            logger.Debug("HTTP server has been stopped.");
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        HttpContext _context = new HttpContext(request, response);

        logger.Debug($"Received request [{request.HttpMethod}] from {request.LocalEndPoint.Address} - {request.Url}");

        await pipeline.Invoke(_context);

        if (_context.Buffer != null)
        {
            using Stream output = response.OutputStream;
            response.ContentLength64 = _context.Buffer.Length;
            await output.WriteAsync(_context.Buffer); 
        }

        response.Close();
        logger.Debug($"Handling request finished with status code: {response.StatusCode}. Response size: {_context.Buffer?.Length ?? 0}");
    }

    public void Stop() => cts.Cancel();
}
