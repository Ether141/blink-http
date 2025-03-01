using BlinkHttp.Handling;
using System.Net;

namespace BlinkHttp.Http
{
    internal class HttpServer
    {
        private readonly HttpListener _listener;
        private readonly string[] _prefixes;

        internal static string WebFolderPath => Path.Combine(Path.GetDirectoryName(Environment.ProcessPath!)!, "web");

        private readonly CancellationTokenSource _cts;

        internal HttpServer(params string[] prefixes)
        {
            _listener = new HttpListener();
            _prefixes = prefixes;

            foreach (var prefix in prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }

            _cts = new CancellationTokenSource();
        }

        internal async Task StartAsync()
        {
            _listener.Start();

            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    Task<HttpListenerContext> contextTask = _listener.GetContextAsync();
                    Task completedTask = await Task.WhenAny(contextTask, Task.Delay(-1, _cts.Token));

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
            catch (HttpListenerException) when (_cts.Token.IsCancellationRequested)
            {
                // OK — listener has stopped
            }
            finally
            {
                _listener.Stop();
                Console.WriteLine("Server has been stopped.");
            }
        }

        private async Task HandleRequestAsync(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            
            Console.WriteLine($"{DateTime.Now}: {request.LocalEndPoint.Address} {request.HttpMethod} {request.Url}");

            byte[] buffer = [];

            GeneralRequestHandler handler = new GeneralRequestHandler();
            handler.HandleRequest(request, response, ref buffer);

            using Stream output = response.OutputStream;
            await output.WriteAsync(buffer);
            response.Close();
        }

        public void Stop() => _cts.Cancel();
    }
}
