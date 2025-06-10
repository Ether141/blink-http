namespace BlinkHttp.Serialization;

/// <summary>
/// The exception which is thrown, when HTTP request body is invalid and cannot be mapped automatically.
/// </summary>
[Serializable]
public class RequestBodyInvalidException : Exception
{
	/// <summary>
	/// Creates new instance of <seealso cref="RequestBodyInvalidException"/>.
	/// </summary>
	public RequestBodyInvalidException() { }

    /// <summary>
    /// Creates new instance of <seealso cref="RequestBodyInvalidException"/>.
    /// </summary>
    public RequestBodyInvalidException(string message) : base(message) { }

    /// <summary>
    /// Creates new instance of <seealso cref="RequestBodyInvalidException"/>.
    /// </summary>
    public RequestBodyInvalidException(string message, Exception inner) : base(message, inner) { }
}
