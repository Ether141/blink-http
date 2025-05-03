using BlinkHttp.Authentication;

namespace BlinkHttp.Http;

/// <summary>
/// Indicates that the endpoint or whole controller requires authorization to be accessed. Also allows defining which users or what roles they must have to access this secured resource.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class AuthorizeAttribute : Attribute
{
    /// <summary>
    /// The collection of selected users allowed to access the resource.
    /// </summary>
    private readonly string[]? selectedUsers;

    /// <summary>
    /// The collection of selected roles allowed to access the resource.
    /// </summary>
    private readonly string[]? selectedRoles;

    /// <summary>
    /// Gets the authentication rules based on the selected users and roles.
    /// Returns <see langword="null"/> if no users or roles are specified.
    /// </summary>
    public AuthenticationRules? AuthenticationRules => selectedUsers != null || selectedRoles != null ? new AuthenticationRules(selectedUsers, selectedRoles) : null;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class with no specific users or roles.
    /// </summary>
    public AuthorizeAttribute() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class with the specified users.
    /// </summary>
    public AuthorizeAttribute(params string[]? users)
    {
        selectedUsers = users;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class with the specified users and roles.
    /// </summary>
    public AuthorizeAttribute(string[]? users = null, string[]? roles = null)
    {
        selectedUsers = users;
        selectedRoles = roles;
    }
}
