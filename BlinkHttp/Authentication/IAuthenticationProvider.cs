namespace BlinkHttp.Authentication;

internal interface IAuthenticationProvider
{
    CredentialsValidationResult ValidateCredentials(string username, string password, out IUser? obtainedUser);
}
