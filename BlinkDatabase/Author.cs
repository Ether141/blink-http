using BlinkDatabase.Annotations;

namespace BlinkDatabase;

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
