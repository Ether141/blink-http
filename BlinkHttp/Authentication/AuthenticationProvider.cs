using BlinkHttp.Authentication.Session;
using System.Net;

namespace BlinkHttp.Authentication;

public class AuthenticationProvider : IAuthenticationProvider
{
    private readonly IUserInfoProvider userInfoProvider;

    public AuthenticationProvider(IUserInfoProvider userInfoProvider)
    {
        this.userInfoProvider = userInfoProvider;
    }

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
