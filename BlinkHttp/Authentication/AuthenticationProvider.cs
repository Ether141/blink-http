using BlinkHttp.Authentication.Session;
using System.Net;

namespace BlinkHttp.Authentication;

/// <summary>
/// Provides methods to validate user credentials using the <see cref="IUserInfoProvider"/>.
/// </summary>
public class AuthenticationProvider : IAuthenticationProvider
{
    private readonly IUserInfoProvider userInfoProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationProvider"/> class.
    /// </summary>
    public AuthenticationProvider(IUserInfoProvider userInfoProvider)
    {
        this.userInfoProvider = userInfoProvider;
    }

    /// <summary>
    /// Validates the provided username and password.
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <param name="password">The password to validate.</param>
    /// <param name="obtainedUser">The user object retrieved during validation, if successful.</param>
    /// <returns>A <see cref="CredentialsValidationResult"/> indicating the result of the validation.</returns>
    public CredentialsValidationResult ValidateCredentials(string username, string password, out IUser? obtainedUser)
    {
        obtainedUser = userInfoProvider.GetUserByUsername(username);

        if (obtainedUser == null)
        {
            return CredentialsValidationResult.UsernameDoesNotExist;
        }

        string hashedPassword = userInfoProvider.GetHashedPassword(obtainedUser.Id)!;

        if (!ValidatePassword(hashedPassword, password))
        {
            return CredentialsValidationResult.PasswordIsWrong;
        }

        return CredentialsValidationResult.Success;
    }

    internal static bool ValidatePassword(string hashedPassword, string plainPassword) => hashedPassword == plainPassword;
}
