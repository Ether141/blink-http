namespace BlinkHttp.Configuration;

/// <summary>
/// Represents errors that occur during loading of application configuration.
/// </summary>
[Serializable]
public class ApplicationConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationConfigurationException"/> class.
    /// </summary>
    public ApplicationConfigurationException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationConfigurationException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ApplicationConfigurationException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationConfigurationException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public ApplicationConfigurationException(string message, Exception inner) : base(message, inner) { }
}
