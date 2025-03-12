namespace BlinkHttp.Authentication.Session;

internal class LoginAttempt
{
    internal int AttemptNum { get; private set; }
    internal long LastAttemptTimestamp { get; private set; }

    private readonly int maxAttemptsPerCooldown = 6;
    private readonly long cooldown = 120;

    public LoginAttempt(long cooldown, int maxAttemptsPerCooldown)
    {
        this.cooldown = cooldown;
        this.maxAttemptsPerCooldown = maxAttemptsPerCooldown;
    }

    public bool RegisterAttempt(long timestamp)
    {
        if (timestamp >= LastAttemptTimestamp + cooldown)
        {
            AttemptNum = 0;
        }

        LastAttemptTimestamp = timestamp;
        return AttemptNum++ < maxAttemptsPerCooldown;
    }
}
