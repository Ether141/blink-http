using BlinkDatabase.Annotations;
using System.Text.Json.Serialization;

namespace BlinkDatabase;

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
