namespace BlinkHttp.Authentication.Session;

/// <summary>
/// Represents session information for a user.
/// </summary>
public class SessionInfo
{
    /// <summary>
    /// Gets the unique identifier for the session.
    /// </summary>
    public string SessionId { get; }

    /// <summary>
    /// Gets the unique identifier for the user associated with the session.
    /// </summary>
    public string UserId { get; }

    /// <summary>
    /// Gets the timestamp indicating when the session was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionInfo"/> class.
    /// </summary>
    public SessionInfo(string sessionId, string userId)
    {
        SessionId = sessionId;
        UserId = userId;
        CreatedAt = DateTime.Now;
    }
}
