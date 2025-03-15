namespace BlinkHttp.Authentication.Session;

/// <summary>
/// Encapsulates options for authorization based on session.
/// </summary>
public class SessionOptions
{
    internal TimeSpan? SessionValidFor { get; private set; }

    /// <summary>
    /// Enables session expiration. With this feauture, every session will expire in given <seealso cref="TimeSpan"/>.
    /// </summary>
    public SessionOptions EnableSessionExpiration(TimeSpan sessionValidFor)
    {
        SessionValidFor = sessionValidFor;
        return this;
    }
}
