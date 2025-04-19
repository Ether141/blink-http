namespace BlinkHttp.Authentication;

/// <summary>
/// Encapsulates information about authentication rules - only selected users, or/and selected roles.
/// </summary>
public class AuthenticationRules
{
    /// <summary>
    /// Gets a value indicating whether only selected users are allowed.
    /// </summary>
    public bool OnlySelectedUsers => SelectedUsers != null && SelectedUsers.Length > 0;

    /// <summary>
    /// Gets a value indicating whether only selected roles are allowed.
    /// </summary>
    public bool OnlySelectedRoles => SelectedRoles != null && SelectedRoles.Length > 0;

    /// <summary>
    /// Gets the collection of selected users. Only those users can access resource.
    /// </summary>
    public string[]? SelectedUsers { get; }

    /// <summary>
    /// Gets the collection of selected roles. Only users who have those roles can access resource.
    /// </summary>
    public string[]? SelectedRoles { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationRules"/> class with specified users and roles.
    /// </summary>
    public AuthenticationRules(string[]? selectedUsers, string[]? selectedRoles)
    {
        SelectedUsers = selectedUsers;
        SelectedRoles = selectedRoles;
    }
}
