using System;

namespace BlinkHttp.Logging;

/// <summary>
/// Exception thrown when logging is attempted but has not been initialized.
/// </summary>
public class LoggingNotInitializedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingNotInitializedException"/> class.
    /// </summary>
    public LoggingNotInitializedException()
        : base("Logging has not been initialized.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingNotInitializedException"/> class with a custom message.
    /// </summary>
    /// <param name="message">The custom error message.</param>
    public LoggingNotInitializedException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingNotInitializedException"/> class with a custom message and an inner exception.
    /// </summary>
    /// <param name="message">The custom error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public LoggingNotInitializedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
