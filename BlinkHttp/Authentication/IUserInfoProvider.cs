namespace BlinkHttp.Authentication;

/// <summary>
/// Exposes methods to obtain <seealso cref="IUser"/> object with information required for authentication process.
/// </summary>
public interface IUserInfoProvider
{
    /// <summary>
    /// Returns <seealso cref="IUser"/> with given ID.
    /// </summary>
    IUser? GetUser(string userId);

    /// <summary>
    /// Returns <seealso cref="IUser"/> with given username.
    /// </summary>
    IUser? GetUserByUsername(string username);

    /// <summary>
    /// Returns <seealso cref="IUser"/> hashed password for user with given ID.
    /// </summary>
    string? GetHashedPassword(string userId);
}
