namespace BlinkDatabase.Mapping.Exceptions;

/// <summary>
/// The exception which is thrown, when none property of model class has <seealso cref="BlinkDatabase.Annotations.IdAttribute"/>.
/// </summary>
[Serializable]
public class IdColumnMissingException : Exception
{
	/// <summary>
	/// Creates new instance of <seealso cref="IdColumnMissingException"/> with information about type of model which is associated with this exception.
	/// </summary>
	public IdColumnMissingException(Type type) : base(GetMessage(type)) { }

    /// <summary>
    /// Creates new instance of <seealso cref="IdColumnMissingException"/> with information about type of model which is associated with this exception and reference to inner exception.
    /// </summary>
    public IdColumnMissingException(Type type, Exception inner) : base(GetMessage(type), inner) { }

	private static string GetMessage(Type type) => $"'{type}' which is supposed to be mapped, must have one property marked with IdAttribute.";
}
