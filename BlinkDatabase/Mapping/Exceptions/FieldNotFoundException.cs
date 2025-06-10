#pragma warning disable CS1591

namespace BlinkDatabase.Mapping.Exceptions;

/// <summary>
/// The exception which is thrown when the field from the database cannot be found.
/// </summary>
[Serializable]
public class FieldNotFoundException : Exception
{
	public FieldNotFoundException(string propertyType) : base(GetMessage(propertyType)) { }

	public FieldNotFoundException(string propertyType, Exception inner) : base(GetMessage(propertyType), inner) { }

	private static string GetMessage(string propertyType) =>
		$"Unable to find field for property '{propertyType}'.";
}
