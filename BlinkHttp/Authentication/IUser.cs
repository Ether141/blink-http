namespace BlinkHttp.Authentication;

/// <summary>
/// Defines properties and methods about a user, who is associated with an HTTP request, mainly required in the authorization process.
/// </summary>
public interface IUser
{
    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the username of the user.
    /// </summary>
    string Username { get; }

    /// <summary>
    /// Gets the roles assigned to the user.
    /// </summary>
    string[] Roles { get; }
}
