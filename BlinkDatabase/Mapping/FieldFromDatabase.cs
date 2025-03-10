namespace BlinkDatabase.Mapping;

internal class FieldFromDatabase
{
    public string FullName { get; }
    public Type FieldType { get; }
    public object Value { get; }
    public string PgsqlType { get; }

    public string FieldName => FullName.Split('.')[^1];

    public FieldFromDatabase(string name, Type fieldType, object value, string pgsqlType)
    {
        FullName = name;
        FieldType = fieldType;
        Value = value;
        PgsqlType = pgsqlType;
    }

    public override string? ToString() => $"{FullName}, {FieldType}, {PgsqlType}";
}
