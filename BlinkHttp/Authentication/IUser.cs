namespace BlinkHttp.Authentication;

/// <summary>
/// Defines properties and methods about user, who is associated with HTTP request, mainly required in authorization process.
/// </summary>
public interface IUser
{
    int Id { get; }
    string Username { get; }
    string[] Roles { get; }

    bool IsInRole(string role);
}
