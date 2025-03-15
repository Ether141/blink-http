using System.Net;

namespace BlinkHttp.Authentication.Additional;

/// <summary>
/// Allows to track failed login attempts and block next attempts if there were too many fails.
/// </summary>
public class LoginAttemptsGuard
{
    private readonly Dictionary<string, LoginAttempt> loggingAttempts = [];

    private int attemptCooldown;
    private int attemptLimit;

    public LoginAttemptsGuard(int attemptCooldown, int attemptLimit)
    {
        this.attemptCooldown = attemptCooldown;
        this.attemptLimit = attemptLimit;
    }

    /// <summary>
    /// Registers failed attempt login from given IP address.
    /// </summary>
    public void RegisterFailedAttempt(string ipAddress)
    {
        long now = DateTimeOffset.Now.ToUnixTimeSeconds();

        if (!loggingAttempts.TryGetValue(ipAddress, out LoginAttempt? loginAttempt))
        {
            loginAttempt = new LoginAttempt(attemptCooldown, attemptLimit);
            loginAttempt.RegisterAttempt(now);
            loggingAttempts[ipAddress] = loginAttempt;
        }
        else
        {
            loginAttempt.RegisterAttempt(now);
        }
    }

    /// <summary>
    /// If user logged in successfully from given IP address, its failed attempts counter should be resetted with this method. 
    /// </summary>
    public void ResetFailedAttempts(string ipAddress)
    {
        if (loggingAttempts.TryGetValue(ipAddress, out LoginAttempt? loginAttempt))
        {
            loggingAttempts[ipAddress].ResetAttempts();
        }
    }

    /// <summary>
    /// Determine if given IP address reached limit of failed login attempts, and if next attempts should be blocked.
    /// </summary>
    public bool ReachedAttemptsLimit(string ipAddress) => loggingAttempts.TryGetValue(ipAddress, out LoginAttempt? loginAttempt) && loginAttempt.ShouldBeBlocked;
}
