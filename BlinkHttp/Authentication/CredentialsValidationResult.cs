namespace BlinkHttp.Authentication;

/// <summary>
/// Information about the result of credentials validation.
/// </summary>
public enum CredentialsValidationResult
{
    /// <summary>
    /// The credentials validation was successful.
    /// </summary>
    Success,

    /// <summary>
    /// The username does not exist in the system.
    /// </summary>
    UsernameDoesNotExist,

    /// <summary>
    /// The provided password is incorrect.
    /// </summary>
    PasswordIsWrong
}
