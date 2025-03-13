namespace BlinkHttp.Authentication;

internal class AuthenticationProvider : IAuthenticationProvider
{
    private readonly IUserInfoProvider userInfoProvider;

    internal AuthenticationProvider(IUserInfoProvider userInfoProvider)
    {
        this.userInfoProvider = userInfoProvider;
    }

    public CredentialsValidationResult ValidateCredentials(string username, string password, out IUser? obtainedUser)
    {
        obtainedUser = null;
        IUser? user = userInfoProvider.GetUser(username);

        if (user == null)
        {
            return CredentialsValidationResult.UsernameDoesNotExist;
        }

        string hashedPassword = userInfoProvider.GetHashedPassword(user.Id)!;
        
        if (!ValidatePassword(hashedPassword, password))
        {
            return CredentialsValidationResult.PasswordIsWrong;
        }

        obtainedUser = user;
        return CredentialsValidationResult.Success;
    }

    internal static bool ValidatePassword(string hashedPassword, string plainPassword) => hashedPassword == plainPassword;
}
