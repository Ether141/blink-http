namespace BlinkHttp.Authentication;

internal interface IUserInfoProvider
{
    IUser? GetUser(string username);
    IUser? GetUser(int userId);
}
