using BlinkDatabase.Annotations;

namespace BlinkDatabase;

[Table("users")]
public class User
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    public string? Username { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public override string? ToString() 
        => $"User(Id = {Id}, Username = {Username}, Email = {Email}, CreatedAt = {CreatedAt})";
}
