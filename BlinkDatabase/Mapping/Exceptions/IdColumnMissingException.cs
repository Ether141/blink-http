namespace BlinkDatabase.Mapping.Exceptions;

[Serializable]
public class IdColumnMissingException : Exception
{
	public IdColumnMissingException(Type type) : base(GetMessage(type)) { }

	public IdColumnMissingException(Type type, Exception inner) : base(GetMessage(type), inner) { }

	private static string GetMessage(Type type) => $"'{type}' which is supposed to be mapped, must have one property marked with IdAttribute.";
}
