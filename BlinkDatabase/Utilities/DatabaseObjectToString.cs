using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;
using System.Reflection;
using System.Text;

namespace BlinkDatabase.Utilities;

public static class DatabaseObjectToString
{
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
