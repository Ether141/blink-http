#pragma warning disable CS8618

using BlinkDatabase.Annotations;
using BlinkHttp.Authentication;

namespace MyApplication;

[Table("users")]
public class User : IUser
{
    [Id]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("nickname")]
    public string Username { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password_hash")]
    public string PasswordHash { get; set; }

    [Column("birth_date")]
    public DateTime BirthDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Relation]
    [Column("profile_id")]
    public UserProfile? Profile { get; set; }

    [Relation]
    [Column("role_id")]
    public Role Role { get; set; }

    public string[] Roles => [Role.RoleName.ToLowerInvariant()];

    string IUser.Id => Id.ToString().ToLowerInvariant();
}

[Table("user_profiles")]
public class UserProfile
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("address")]
    public string Address { get; set; }

    [Column("phone_number")]
    public string PhoneNumber { get; set; }

    [Column("bio")]
    public string Bio { get; set; }
}

[Table("roles")]
public class Role
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("role_name")]
    public string RoleName { get; set; }

    [Column("description")]
    public string Description { get; set; }
}

[Table("books")]
public class Book
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Relation("id")]
    [Column("library_id")]
    public Library Library { get; set; }

    [Relation("id")]
    [Column("author_id")]
    public Author? Author { get; set; }

    public override string ToString() => $"Book(Id = {Id}, Name = {Name}, LibraryId = {Library?.Id}, Author = {Author})";
}

[Table("libraries")]
public class Library
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("type")]
    public LibraryType Type { get; set; }

    [Relation("library_id")]
    [Column("id")]
    public List<Book>? Books { get; set; }

    public override string ToString() => $"Library(Id = {Id}, Name = {Name}, Type = {Type}, Books = [{(Books != null ? string.Join(", ", Books) : "N/A")}])";
}

[Enum("book_type")]
public enum LibraryType
{
    [EnumValue("buy")]
    Buy,

    [EnumValue("rent")]
    Rent,

    [EnumValue("free")]
    Free
}

[Table("authors")]
public class Author
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("birthdate")]
    public DateTime Birthdate { get; set; }

    public override string ToString() => $"Author(Id = {Id}, Name = {Name}, Birthdate = {Birthdate.ToShortDateString()})";
}