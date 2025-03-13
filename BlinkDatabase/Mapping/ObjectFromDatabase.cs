namespace BlinkDatabase.Mapping;

internal class ObjectFromDatabase
{
    internal List<FieldFromDatabase> Fields { get; }
    internal int Id
    {
        get
        {
            if (id != null)
                return id.Value;

            id = int.Parse(Fields.FirstOrDefault(f => f.FieldName.Equals("id", StringComparison.OrdinalIgnoreCase))!.Value.ToString()!);
            return id.Value;
        }
    }

    private int? id;

    internal ObjectFromDatabase(List<FieldFromDatabase> fields)
    {
        Fields = fields;
    }
}
