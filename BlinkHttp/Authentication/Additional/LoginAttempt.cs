namespace BlinkHttp.Authentication.Additional;

internal class LoginAttempt
{
    internal int AttemptNum { get; private set; }
    internal long LastAttemptTimestamp { get; private set; }

    private readonly int maxAttemptsPerCooldown;
    private readonly long cooldown;

    internal bool ShouldBeBlocked
    {
        get
        {
            if (LastAttemptTimestamp + cooldown <= DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                ResetAttempts();
            }

            return AttemptNum >= maxAttemptsPerCooldown;
        }
    }

    internal LoginAttempt(long cooldown, int maxAttemptsPerCooldown)
    {
        this.cooldown = cooldown;
        this.maxAttemptsPerCooldown = maxAttemptsPerCooldown;
    }

    internal void RegisterAttempt(long timestamp)
    {
        if (timestamp >= LastAttemptTimestamp + cooldown)
        {
            AttemptNum = 0;
        }

        LastAttemptTimestamp = timestamp;
        AttemptNum++;
    }

    internal void ResetAttempts() => LastAttemptTimestamp = AttemptNum = 0;
}
