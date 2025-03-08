namespace BlinkDatabase.Mapping.Exceptions;


[Serializable]
public class PropertyTypeMismatchException : Exception
{
	public PropertyTypeMismatchException(string columnName, string propertyName, Type columnType, Type propertyType) 
		: base(GetMessage(columnName, propertyName, columnType, propertyType)) { }

	public PropertyTypeMismatchException(string columnName, string propertyName, Type columnType, Type propertyType, Exception inner) 
		: base(GetMessage(columnName, propertyName, columnType, propertyType), inner) { }

	private static string GetMessage(string columnName, string propertyName, Type columnType, Type propertyType) =>
		$"Unable to assign value to property ({propertyName}) from column ({columnName}) because of type mismatch: {propertyType} =/= {columnType}";
}
