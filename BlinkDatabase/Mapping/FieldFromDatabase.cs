namespace BlinkDatabase.Mapping;

internal class FieldFromDatabase
{
    internal string FullName { get; }
    internal Type FieldType { get; }
    internal object Value { get; }
    internal string SqlType { get; }

    internal string FieldName => FullName.Split('.')[^1];

    internal FieldFromDatabase(string name, Type fieldType, object value, string pgsqlType)
    {
        FullName = name;
        FieldType = fieldType;
        Value = value;
        SqlType = pgsqlType;
    }

    public override string? ToString() => $"{FullName}, {FieldType}, {SqlType}";
}
