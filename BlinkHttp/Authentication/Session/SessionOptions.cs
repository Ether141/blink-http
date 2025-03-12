namespace BlinkHttp.Authentication.Session;

public class SessionOptions
{
    internal bool AttemptsLimitingEnabled { get; private set; }
    internal int AttemptsLimitingCooldown { get; private set; }
    internal int AttemptsLimitPerCooldown { get; private set; }
    internal TimeSpan? SessionValidFor { get; private set; }

    public SessionOptions EnableAttemptsLimiting(int cooldown, int limit)
    {
        AttemptsLimitingEnabled = true;
        AttemptsLimitingCooldown = cooldown;
        AttemptsLimitPerCooldown = limit;
        return this;
    }

    public SessionOptions EnableSessionExpiration(TimeSpan sessionValidFor)
    {
        SessionValidFor = sessionValidFor;
        return this;
    }
}
