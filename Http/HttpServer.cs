using BlinkHttp.Handling;
using BlinkHttp.Routing;
using System.Net;

namespace BlinkHttp.Http;

internal class HttpServer
{
    private readonly HttpListener listener;
    private readonly Router router;
    private readonly GeneralRequestHandler generalHandler;
    private readonly string[] prefixes;

    internal IReadOnlyList<string> Prefixes => prefixes;
    internal static string WebFolderPath => Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "web");

    private readonly CancellationTokenSource cts;

    internal HttpServer(params string[] prefixes)
    {
        listener = new HttpListener();

        this.prefixes = prefixes;

        foreach (var prefix in prefixes)
        {
            listener.Prefixes.Add(prefix);
        }

        cts = new CancellationTokenSource();

        router = ConfigureRouter();
        generalHandler = new GeneralRequestHandler(router);
    }

    private static Router ConfigureRouter()
    {
        Router router = new Router();
        router.Options.RoutePrefix = "api";
        router.InitializeAllRoutes();
        return router;
    }

    internal async Task StartAsync()
    {
        listener.Start();

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
            Console.WriteLine("Server has been stopped.");
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        HttpContext httpContext = new HttpContext(request, response);

        Console.WriteLine($"{DateTime.Now}: {request.LocalEndPoint.Address} {request.HttpMethod} {request.Url}");

        byte[] buffer = [];

        generalHandler.HandleRequest(httpContext, ref buffer);

        using Stream output = response.OutputStream;
        await output.WriteAsync(buffer);
        response.Close();
    }

    public void Stop() => cts.Cancel();
}
