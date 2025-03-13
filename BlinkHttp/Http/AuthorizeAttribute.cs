using BlinkHttp.Authentication;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that the endpoint or whole controller requires authorization to be accessed. Also allows to define which user or what roles they must have to access this secured resource.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class AuthorizeAttribute : Attribute 
{
    private readonly string[]? selectedUsers;
    private readonly string[]? selectedRoles;

    public AuthenticationRules? AuthenticationRules => selectedUsers != null || selectedRoles != null ? new AuthenticationRules(selectedUsers, selectedRoles) : null;

    public AuthorizeAttribute() { }

    public AuthorizeAttribute(params string[]? users)
    {
        selectedUsers = users;
    }

    public AuthorizeAttribute(string[]? users = null, string[]? roles = null)
    {
        selectedUsers = users;
        selectedRoles = roles;
    }
}
