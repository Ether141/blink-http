using BlinkDatabase.Annotations;

namespace BlinkDatabase;

[Table("cities")]
public class City
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("zip_code")]
    public string? ZipCode { get; set; }

    public override string? ToString()
        => $"City(Id = {Id}, Name = {Name}, ZipCode = {ZipCode})";
}
