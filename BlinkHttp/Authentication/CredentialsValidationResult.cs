namespace BlinkHttp.Authentication;

/// <summary>
/// Information about result of credentials validation.
/// </summary>
public enum CredentialsValidationResult
{
    Success,
    UsernameDoesNotExist,
    PasswordIsWrong,
    TooManyRequests
}
