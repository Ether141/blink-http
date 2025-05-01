using System.Net;

namespace BlinkHttp.Server;

internal interface IServer
{
    event Func<HttpServerContext, Task>? RequestReceived;

    Task StartAsync();
    void Stop();
}
