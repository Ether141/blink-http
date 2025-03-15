namespace BlinkHttp.Authentication;

public interface IAuthenticationProvider
{
    CredentialsValidationResult ValidateCredentials(string username, string password, out IUser? obtainedUser);
}
