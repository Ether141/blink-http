namespace BlinkHttp.Authentication;

internal class DatabaseUserInfoProvider : IUserInfoProvider
{
    private readonly IUser[] users =
    [
        new User() { Id = 1, Username = "exampleUser123", Roles = ["user"] },
        new User() { Id = 2, Username = "admin", Roles = ["user", "admin"] }
    ];

    private readonly Dictionary<int, string> passwords = new()
    {
        { 1, "hashed_password" },
        { 2, "hashed_password" }
    };

    public IUser? GetUser(string username) => users.FirstOrDefault(x => x.Username == username);

    public string? GetHashedPassword(int userId) => passwords.TryGetValue(userId, out string? pass) ? pass : null;

    public IUser? GetUser(int userId) => users.FirstOrDefault(x => x.Id == userId);
}
