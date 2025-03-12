namespace BlinkHttp.Authentication.Session;

internal static class SessionIdProvider
{
    internal static string GenerateSessionId()
    {
        string id = Guid.NewGuid().ToString().ToLowerInvariant().Replace("-", "");
        for (int i = 1; i <= 3; i++)
        {
            id = id.Insert(i * 8 + i - 1, "-");
        }
        return id;
    }
}
