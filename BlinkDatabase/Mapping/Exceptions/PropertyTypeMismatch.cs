namespace BlinkDatabase.Mapping.Exceptions;

/// <summary>
/// The exception which is thrown when data from the database has different type than property of the model.
/// </summary>
[Serializable]
public class PropertyTypeMismatchException : Exception
{
	/// <summary>
	/// Creates new instance of <seealso cref="PropertyTypeMismatchException"/>.
	/// </summary>
	public PropertyTypeMismatchException(string columnName, string propertyName, Type columnType, Type propertyType) 
		: base(GetMessage(columnName, propertyName, columnType, propertyType)) { }

    /// <summary>
    /// Creates new instance of <seealso cref="PropertyTypeMismatchException"/>.
    /// </summary>
    public PropertyTypeMismatchException(string columnName, string propertyName, Type columnType, Type propertyType, Exception inner) 
		: base(GetMessage(columnName, propertyName, columnType, propertyType), inner) { }

	private static string GetMessage(string columnName, string propertyName, Type columnType, Type propertyType) =>
		$"Unable to assign value to property ({propertyName}) from column ({columnName}) because of type mismatch: {propertyType} =/= {columnType}";
}
