namespace BlinkHttp.Authentication;

public enum CredentialsValidationResult
{
    Success,
    UsernameDoesNotExist,
    PasswordIsWrong,
    TooManyRequests
}
