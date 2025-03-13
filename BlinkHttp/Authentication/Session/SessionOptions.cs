namespace BlinkHttp.Authentication.Session;

/// <summary>
/// Encapsulates options for authorization based on session.
/// </summary>
public class SessionOptions
{
    internal bool AttemptsLimitingEnabled { get; private set; }
    internal int AttemptsLimitingCooldown { get; private set; }
    internal int AttemptsLimitPerCooldown { get; private set; }
    internal TimeSpan? SessionValidFor { get; private set; }

    /// <summary>
    /// Enables attempts limiting feature. If number of failed requests about logging in will reach limit, this feature will block all requests for cooldown.
    /// </summary>
    public SessionOptions EnableAttemptsLimiting(int cooldown, int limit)
    {
        AttemptsLimitingEnabled = true;
        AttemptsLimitingCooldown = cooldown;
        AttemptsLimitPerCooldown = limit;
        return this;
    }

    /// <summary>
    /// Enables session expiration. With this feauture, every session will expire in given <seealso cref="TimeSpan"/>.
    /// </summary>
    public SessionOptions EnableSessionExpiration(TimeSpan sessionValidFor)
    {
        SessionValidFor = sessionValidFor;
        return this;
    }
}
