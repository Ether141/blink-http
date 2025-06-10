namespace BlinkDatabase.Mapping.Exceptions;

/// <summary>
/// The exception which is thrown when <seealso cref="BlinkDatabase.Annotations.TableAttribute"/> is missing on model class.
/// </summary>
[Serializable]
public class TableAttributeMissingException : Exception
{
	/// <summary>
	/// Creates new instance of <seealso cref="TableAttributeMissingException"/>.
	/// </summary>
	public TableAttributeMissingException(string className) : base($"TableAttribute is missing on {className}") { }

    /// <summary>
    /// Creates new instance of <seealso cref="TableAttributeMissingException"/>.
    /// </summary>
    public TableAttributeMissingException(string className, Exception inner) : base($"TableAttribute is missing on {className}", inner) { }
}
