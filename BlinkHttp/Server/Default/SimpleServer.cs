using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using BlinkHttp.Handling.Pipeline;
using BlinkHttp.Handling;
using BlinkHttp.Routing;
using BlinkHttp.Logging;
using System.Net;
using BlinkHttp.Http;

namespace BlinkHttp.Server.Default;

internal class SimpleServer : IServer
{
    private readonly HttpListener listener;

    private readonly ILogger logger = LoggerFactory.Create<SimpleServer>();
    private readonly CancellationTokenSource cts;

    public event Func<HttpServerContext, Task>? RequestReceived;

    internal SimpleServer(params string[] prefixes)
    {
        logger.Debug("Initializing simple HTTP server (System.Net.HttpListener)");

        listener = new HttpListener();

        foreach (string prefix in prefixes)
        {
            listener.Prefixes.Add(prefix);
        }

        logger.Debug($"Prefixes: {string.Join(", ", prefixes)}");
        cts = new CancellationTokenSource();
    }

    public async Task StartAsync()
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
        if (RequestReceived == null)
        {
            return;
        }

        await RequestReceived.Invoke(context.Wrap());
    }

    public void Stop() => cts.Cancel();
}
