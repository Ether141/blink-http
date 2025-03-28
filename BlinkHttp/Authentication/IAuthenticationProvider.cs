namespace BlinkHttp.Authentication;

/// <summary>
/// Exposes method to validate user credentials, in order to e.g. login user.
/// </summary>
public interface IAuthenticationProvider
{
    /// <summary>
    /// Validates given username and password, using provided <seealso cref="IUserInfoProvider"/>. Returns <seealso cref="CredentialsValidationResult"/> and optionally <seealso cref="IUser"/> if validations was successful.
    /// </summary>
    CredentialsValidationResult ValidateCredentials(string username, string password, out IUser? obtainedUser);
}
