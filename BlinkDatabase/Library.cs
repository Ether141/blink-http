using BlinkDatabase.Annotations;

namespace BlinkDatabase;

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

    public override string ToString() => $"Library(Id = {Id}, Name = {Name}, Type = {Type}, Books = [{string.Join(", ", Books!)}])";
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