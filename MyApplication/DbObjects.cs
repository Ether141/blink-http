using BlinkDatabase.Annotations;

namespace MyApplication;

[Table("users")]
public class User
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("nickname")]
    public string? Nickname { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("password_hash")]
    public string? PasswordHash { get; set; }

    [Column("birth_date")]
    public DateTime BirthDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Relation]
    [Column("profile_id")]
    public UserProfile? Profile { get; set; }

    [Relation]
    [Column("role_id")]
    public Role? Role { get; set; }
}

[Table("user_profiles")]
public class UserProfile
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("bio")]
    public string? Bio { get; set; }
}

[Table("roles")]
public class Role
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("role_name")]
    public string? RoleName { get; set; }

    [Column("description")]
    public string? Description { get; set; }
}

[Table("user_role_history")]
public class UserRoleHistory
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("changed_at")]
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    [Relation]
    [Column("user_id")]
    public User? User { get; set; }

    [Relation]
    [Column("old_role_id")]
    public Role? OldRole { get; set; }

    [Relation]
    [Column("new_role_id")]
    public Role? NewRole { get; set; }
}
