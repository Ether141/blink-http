using System.Net;

namespace BlinkHttp.Http;

/// <summary>
/// Represents a response to a request being handled by an <see cref="HttpListener"/>.
/// </summary>
public abstract class HttpResponse
{
    /// <summary>
    /// Gets a <see cref="Stream"/> object to which a response can be written.
    /// </summary>
    public abstract Stream OutputStream { get; }

    /// <summary>
    /// Gets or sets the HTTP status code to be returned to the client.
    /// </summary>
    /// <value>
    /// An <see cref="int"/> value that specifies the HTTP status code for the response.
    /// </value>
    public abstract int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the number of bytes in the body data included in the response.
    /// </summary>
    /// <value>
    /// A <see cref="long"/> value that specifies the content length of the response.
    /// </value>
    public abstract long ContentLength64 { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the content returned in the response.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the content type of the response, or <c>null</c> if not set.
    /// </value>
    public abstract string? ContentType { get; set; }

    /// <summary>
    /// Gets the collection of cookies returned with the response.
    /// </summary>
    /// <value>
    /// A <see cref="CookieCollection"/> containing the cookies associated with the response.
    /// </value>
    public abstract CookieCollection Cookies { get; }

    /// <summary>
    /// Sends the response to the client and releases the resources held by this <see cref="HttpResponse"/> instance.
    /// </summary>
    public abstract void Close();

    /// <summary>
    /// Adds a header with the specified name and value to the response.
    /// </summary>
    /// <param name="name">The name of the header to add.</param>
    /// <param name="value">The value of the header to add.</param>
    public abstract void AddHeader(string name, string value);

    /// <summary>
    /// Gets or sets the HTTP version used for the response.
    /// </summary>
    /// <value>
    /// A <see cref="Version"/> object representing the HTTP version of the response.
    /// </value>
    public abstract Version ProtocolVersion { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the server requests a persistent connection.
    /// </summary>
    /// <value>
    /// <c>true</c> if the server requests a persistent connection; otherwise, <c>false</c>.
    /// </value>
    public abstract bool KeepAlive { get; set; }

    /// <summary>
    /// Gets or sets the collection of header name/value pairs returned by the server.
    /// </summary>
    /// <value>
    /// A <see cref="WebHeaderCollection"/> containing the headers for the response.
    /// </value>
    public abstract WebHeaderCollection Headers { get; set; }

    /// <summary>
    /// Configures the response to redirect the client to the specified URL.
    /// </summary>
    /// <param name="url">The URL to which the client is redirected.</param>
    public abstract void Redirect(string url);

    /// <summary>
    /// Gets or sets a value indicating whether the response uses chunked transfer encoding.
    /// </summary>
    /// <value>
    /// <c>true</c> if the response uses chunked transfer encoding; otherwise, <c>false</c>.
    /// </value>
    public abstract bool SendChunked { get; set; }

    /// <summary>
    /// Appends a value to the specified HTTP header to be sent with this response.
    /// </summary>
    /// <param name="name">The name of the header to append to.</param>
    /// <param name="value">The value to append to the header.</param>
    public abstract void AppendHeader(string name, string value);

    /// <summary>
    /// Appends a cookie to the response.
    /// </summary>
    /// <param name="cookie">The <see cref="Cookie"/> to append to the response.</param>
    public abstract void AppendCookie(Cookie cookie);
}
