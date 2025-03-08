namespace BlinkDatabase.Mapping;

internal class ObjectFromDatabase
{
    internal List<FieldFromDatabase> Fields { get; }
    internal int Id { get; }

    public ObjectFromDatabase(List<FieldFromDatabase> fields)
    {
        Fields = fields;
        Id = int.Parse(fields.FirstOrDefault(f => f.FieldName.Equals("id", StringComparison.OrdinalIgnoreCase))!.Value.ToString()!);
    }
}
