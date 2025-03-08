using BlinkDatabase.Annotations;

namespace BlinkDatabase;

[Table("addresses")]
public class Address
{
    [Id]
    [Column("id")]
    public int Id { get; set; }

    [Relation("id")]
    [Column("city_id")]
    public City? City { get; set; }

    [Column("street")]
    public string? Street { get; set; }

    public override string? ToString()
        => $"Address(Id = {Id}, City = {City}, Street = {Street})";
}
