using System.Net;

namespace BlinkHttp.Handling;

/// <summary>
/// Represents the base class for middleware components that can handle HTTP requests and responses.
/// </summary>
public interface IMiddleware
{
    /// <summary>
    /// Handles the incoming HTTP request and produces a corresponding HTTP response. 
    /// Returns true if request was handled and pipeline can move on to the next middleware. Otherwise returns false, which tells server to break the pipeline and send the response to the client.
    /// </summary>
    bool Handle(HttpListenerRequest request, HttpListenerResponse response);
}
