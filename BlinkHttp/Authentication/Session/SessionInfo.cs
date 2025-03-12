namespace BlinkHttp.Authentication.Session;

public class SessionInfo
{
    public string SessionId { get; }
    public int UserId { get; }
    public DateTime CreatedAt { get; }

    public SessionInfo(string sessionId, int userId)
    {
        SessionId = sessionId;
        UserId = userId;
        CreatedAt = DateTime.Now;
    }
}
