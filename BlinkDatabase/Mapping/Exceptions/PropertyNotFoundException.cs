namespace BlinkDatabase.Mapping.Exceptions;

[Serializable]
public class PropertyNotFoundException : Exception
{
	public PropertyNotFoundException(string columnName) : base(GetMessage(columnName)) { }

	public PropertyNotFoundException(string columnName, Exception inner) : base(GetMessage(columnName), inner) { }

	private static string GetMessage(string columnName) =>
		$"Unable to find property for column '{columnName}'.";
}
