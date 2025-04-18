﻿using BlinkDatabase.General;
using BlinkHttp.Authentication;
using BlinkHttp.Configuration;
using BlinkHttp.Handling;
using BlinkHttp.Routing;
using Logging;
using System.Net;

namespace BlinkHttp.Http;

internal class HttpServer
{
    private readonly HttpListener listener;
    private readonly Router router;
    private readonly GeneralRequestHandler generalHandler;
    private readonly MiddlewareHandler middlewareHandler;
    private readonly IAuthorizer? authorizer;
    private readonly string[] prefixes;
    private readonly ILogger logger = Logger.GetLogger<HttpServer>();
    private readonly string? routePrefix;

    internal IReadOnlyList<string> Prefixes => prefixes;
    internal static string WebFolderPath => Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "web");

    private readonly CancellationTokenSource cts;

    internal HttpServer(IAuthorizer? authorizer, IConfiguration? configuration, MiddlewareHandler middlewareHandler, string? routePrefix, params string[] prefixes)
    {
        logger.Debug("Initializing HTTP server...");

        listener = new HttpListener();

        this.authorizer = authorizer;
        this.middlewareHandler = middlewareHandler;
        this.prefixes = prefixes;
        this.routePrefix = routePrefix;

        foreach (string prefix in prefixes)
        {
            listener.Prefixes.Add(prefix);
        }

        logger.Debug($"Prefixes: {string.Join(", ", prefixes)}");

        cts = new CancellationTokenSource();

        router = ConfigureRouter();
        generalHandler = new GeneralRequestHandler(router, authorizer, configuration);
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
        ControllerContext httpContext = new ControllerContext(request, response);

        logger.Debug($"Received request [{request.HttpMethod}] from {request.LocalEndPoint.Address} - {request.Url}");

        bool middlewareDone = middlewareHandler.Handle(request, response);
        byte[] buffer = [];

        if (middlewareDone)
        {
            generalHandler.HandleRequest(httpContext, ref buffer);
            using Stream output = response.OutputStream;
            await output.WriteAsync(buffer);
        }
        else
        {
            logger.Debug("Middleware pipeline was interrupted.");
        }

        response.Close();

        logger.Debug($"Handling request finished with status code: {response.StatusCode}. Response size: {buffer.Length}");
    }

    public void Stop() => cts.Cancel();
}
