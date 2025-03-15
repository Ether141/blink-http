namespace BlinkDatabase.Mapping;

internal class ObjectFromDatabase
{
    internal List<FieldFromDatabase> Fields { get; }
    internal string Id
    {
        get
        {
            if (id != null)
                return id;

            id = Fields.FirstOrDefault(f => f.FieldName.Equals("id", StringComparison.OrdinalIgnoreCase))!.Value.ToString()!;
            return id;
        }
    }

    private string? id;

    internal ObjectFromDatabase(List<FieldFromDatabase> fields)
    {
        Fields = fields;
    }
}
