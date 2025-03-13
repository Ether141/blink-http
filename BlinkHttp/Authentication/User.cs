using System.Security.Claims;

namespace BlinkHttp.Authentication;

internal class User : IUser
{
    public int Id { get; internal set; }

    public string Username { get; internal set; }

    public string[] Roles { get; internal set; }

    public bool IsInRole(string role) => Roles != null && Roles.Any(r => r == role);
}
