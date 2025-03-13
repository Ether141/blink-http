namespace BlinkHttp.Authentication;

/// <summary>
/// Encapsulates information about authentication rules - only selected users, or/and selected roles.
/// </summary>
public class AuthenticationRules
{
    public bool OnlySelectedUsers => SelectedUsers != null && SelectedUsers.Length > 0;
    public bool OnlySelectedRoles => SelectedRoles != null && SelectedRoles.Length > 0;

    public string[]? SelectedUsers { get; }
    public string[]? SelectedRoles { get; }

    public AuthenticationRules(string[]? selectedUsers, string[]? selectedRoles)
    {
        SelectedUsers = selectedUsers;
        SelectedRoles = selectedRoles;
    }
}
