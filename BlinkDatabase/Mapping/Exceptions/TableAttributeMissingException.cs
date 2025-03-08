namespace BlinkDatabase.Mapping.Exceptions;

[Serializable]
public class TableAttributeMissingException : Exception
{
	public TableAttributeMissingException(string className) : base($"TableAttribute is missing on {className}") { }

	public TableAttributeMissingException(string className, Exception inner) : base($"TableAttribute is missing on {className}", inner) { }
}
