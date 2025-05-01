using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace BlinkHttp.Http;

/// <summary>
/// Provides access to the HTTP request information sent by a client.
/// </summary>
public abstract class HttpRequest
{
    /// <summary>
    /// Gets the HTTP method specified by the client.
    /// </summary>
    /// <returns>A <see cref="string"/> that contains the method used in the request.</returns>
    public abstract string HttpMethod { get; }

    /// <summary>
    /// Gets the local endpoint to which the request is directed.
    /// </summary>
    /// <returns>An <see cref="IPEndPoint"/> that represents the local endpoint.</returns>
    public abstract IPEndPoint LocalEndPoint { get; }

    /// <summary>
    /// Gets the URL requested by the client.
    /// </summary>
    /// <returns>A <see cref="Uri"/> that contains the URL of the request.</returns>
    public abstract Uri? Url { get; }

    /// <summary>
    /// Gets the collection of header name/value pairs sent in the request.
    /// </summary>
    /// <returns>A <see cref="NameValueCollection"/> containing the headers.</returns>
    public abstract NameValueCollection Headers { get; }

    /// <summary>
    /// Gets the cookies sent with the request.
    /// </summary>
    /// <returns>A <see cref="CookieCollection"/> containing the cookies.</returns>
    public abstract CookieCollection Cookies { get; }

    /// <summary>
    /// Gets the client IP address and port number from which the request originated.
    /// </summary>
    /// <returns>An <see cref="IPEndPoint"/> representing the remote endpoint.</returns>
    public abstract IPEndPoint RemoteEndPoint { get; }

    /// <summary>
    /// Gets the content length of the request, if known.
    /// </summary>
    /// <returns>A <see cref="long"/> representing the content length.</returns>
    public abstract long ContentLength64 { get; }

    /// <summary>
    /// Gets the MIME type of the body data included in the request.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the content type, or <c>null</c> if not specified.</returns>
    public abstract string? ContentType { get; }

    /// <summary>
    /// Gets the character encoding of the body data included in the request.
    /// </summary>
    /// <returns>An <see cref="Encoding"/> representing the content encoding.</returns>
    public abstract Encoding ContentEncoding { get; }

    /// <summary>
    /// Gets a value indicating whether the request has associated body data.
    /// </summary>
    /// <returns><c>true</c> if the request has body data; otherwise, <c>false</c>.</returns>
    public abstract bool HasEntityBody { get; }

    /// <summary>
    /// Gets a stream that contains the body data sent by the client.
    /// </summary>
    /// <returns>A <see cref="Stream"/> containing the body data.</returns>
    public abstract Stream InputStream { get; }

    /// <summary>
    /// Gets the HTTP version used by the client.
    /// </summary>
    /// <returns>A <see cref="Version"/> representing the protocol version.</returns>
    public abstract Version ProtocolVersion { get; }

    /// <summary>
    /// Gets a value indicating whether the request is authenticated.
    /// </summary>
    /// <returns><c>true</c> if the request is authenticated; otherwise, <c>false</c>.</returns>
    public abstract bool IsAuthenticated { get; }

    /// <summary>
    /// Gets a value indicating whether the request is sent from a local computer.
    /// </summary>
    /// <returns><c>true</c> if the request is local; otherwise, <c>false</c>.</returns>
    public abstract bool IsLocal { get; }

    /// <summary>
    /// Gets a value indicating whether the request was sent using HTTPS.
    /// </summary>
    /// <returns><c>true</c> if the request is secure; otherwise, <c>false</c>.</returns>
    public abstract bool IsSecureConnection { get; }

    /// <summary>
    /// Gets the HTTP method used to establish a connection to the server.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the service name, or <c>null</c> if not specified.</returns>
    public abstract string? ServiceName { get; }

    /// <summary>
    /// Gets the user agent presented by the client.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the user agent, or <c>null</c> if not specified.</returns>
    public abstract string? UserAgent { get; }

    /// <summary>
    /// Gets the server IP address and port number to which the request is directed.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the user host address, or <c>null</c> if not specified.</returns>
    public abstract string? UserHostAddress { get; }

    /// <summary>
    /// Gets the DNS name and, if provided, the port number specified by the client.
    /// </summary>
    /// <returns>A <see cref="string"/> representing the user host name, or <c>null</c> if not specified.</returns>
    public abstract string? UserHostName { get; }

    /// <summary>
    /// Gets the natural languages that are preferred for the response.
    /// </summary>
    /// <returns>An array of <see cref="string"/> representing the preferred languages, or <c>null</c> if not specified.</returns>
    public abstract string[]? UserLanguages { get; }
}
