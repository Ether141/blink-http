using BlinkDatabase.Annotations;

namespace BlinkDatabase;

[Table("books")]
public class Book
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Relation("id")]
    [Column("library_id")]
    public Library? Library { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Relation("id")]
    [Column("author_id")]
    public Author? Author { get; set; }

    public override string ToString() => $"Book(Id = {Id}, Name = {Name}, Author = {Author})";
}
