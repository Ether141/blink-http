using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;
using System.Reflection;
using System.Text;

namespace BlinkDatabase.Utilities;

/// <summary>
/// Provides functionality to convert database objects to their string representation.
/// </summary>
public static class DatabaseObjectToString
{
    /// <summary>
    /// Converts the given database object to a string representation, including its properties.
    /// </summary>
    /// <param name="obj">The database object to convert.</param>
    /// <param name="fullRelationObjects">
    /// If set to <c>true</c>, full relation objects are serialized recursively.
    /// If <c>false</c>, only the identifier property of the relation object is used.
    /// </param>
    /// <returns>A string representation of the database object.</returns>
    public static string ToString(object obj, bool fullRelationObjects = true)
    {
        string tableName = obj.GetType().GetCustomAttribute<TableAttribute>()!.TableName;
        ObjectProperty[] properties = ObjectProperty.GetProperties(obj.GetType());
        StringBuilder stringBuilder = new StringBuilder($"{obj.GetType().Name}(");

        foreach (ObjectProperty prop in properties)
        {
            string value = prop.GetAsSqlString(obj);

            if (prop.IsRelation)
            {
                if (fullRelationObjects)
                {
                    value = ToString(prop.Get(obj)!);
                }
                else
                {
                    value = ObjectProperty.GetIdProperty(prop.StoredType).Get(prop.Get(obj)!)!.ToString()!;
                }
            }

            stringBuilder.Append($"{prop.Name} = {value}, ");
        }

        stringBuilder.Append(')');
        return stringBuilder.ToString()[..^2];
    }
}
