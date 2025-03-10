namespace BlinkDatabase.Mapping.Exceptions;

[Serializable]
public class FieldNotFoundException : Exception
{
	public FieldNotFoundException(string propertyType) : base(GetMessage(propertyType)) { }

	public FieldNotFoundException(string propertyType, Exception inner) : base(GetMessage(propertyType), inner) { }

	private static string GetMessage(string propertyType) =>
		$"Unable to find field for property '{propertyType}'.";
}
