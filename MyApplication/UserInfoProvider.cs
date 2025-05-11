using BlinkDatabase.General;
using BlinkHttp.Authentication;

namespace MyApplication;

public class UserInfoProvider : IUserInfoProvider
{
    private readonly IRepository<User> repository;

    public UserInfoProvider(IRepository<User> repository)
    {
        this.repository = repository;
    }

    public string? GetHashedPassword(string userId) => repository.SelectSingle(u => u.Id.ToString().ToLowerInvariant() == userId)?.PasswordHash;

    public IUser? GetUser(string id) => repository.SelectSingle(u => u.Id.ToString().ToLowerInvariant() == id);

    public IUser? GetUserByUsername(string username) => repository.SelectSingle(u => u.Username == username);
}
