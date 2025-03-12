namespace BlinkHttp.Authentication;

internal class DatabaseUserInfoProvider : IUserInfoProvider
{
    private readonly IUser[] users =
    [
        new User() { Id = 1, Username = "exampleUser123", Password = "hashed_password", Roles = ["user"] },
        new User() { Id = 2, Username = "admin", Password = "hashed_password", Roles = ["user", "admin"] }
    ];

    public IUser? GetUser(string username) => users.FirstOrDefault(x => x.Username == username);

    public IUser? GetUser(int userId) => users.FirstOrDefault(x => x.Id == userId);
}
