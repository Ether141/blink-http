namespace BlinkHttp.Authentication.Session;

/// <summary>
/// Exception that is thrown when a session with the given ID already exists.
/// </summary>
[Serializable]
public class SessionIdAlreadyExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionIdAlreadyExistsException"/> class
    /// with a default error message.
    /// </summary>
    public SessionIdAlreadyExistsException()
        : base("Session with given ID already exists.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionIdAlreadyExistsException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public SessionIdAlreadyExistsException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionIdAlreadyExistsException"/> class
    /// with a specified error message and a reference to the inner exception that is the cause
    /// of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public SessionIdAlreadyExistsException(string message, Exception inner)
        : base(message, inner) { }
}
