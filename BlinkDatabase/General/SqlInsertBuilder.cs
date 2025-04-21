using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;

namespace BlinkDatabase.General;

internal static class SqlInsertBuilder
{
    internal static string Insert<T>(T obj) where T : class, new()
    {
        ObjectProperty[] properties = [.. ObjectProperty.GetProperties<T>().Where(p => !p.IsId)];
        string idColumnName = ObjectProperty.GetProperties<T>().First(p => p.IsId).FullName;
        string tableName = TableAttribute.GetTableName<T>();
        string valueNames = "";
        string values = "";

        foreach (ObjectProperty prop in properties)
        {
            if (prop.RelationType == RelationType.OneToMany)
            {
                continue;
            }
            
            valueNames += $"\"{prop.ColumnName}\", ";

            if (!prop.IsRelation)
            {
                values += $"{prop.GetAsSqlString(obj)!}, ";
                continue;
            }

            object objFromRelation = prop.Get(obj)!;
            object relationId = ObjectProperty.GetIdProperty(objFromRelation.GetType()).Get(objFromRelation)!;
            values += $"{relationId}, ";
        }

        string query = $"INSERT INTO \"{tableName}\" ({valueNames[..^2]}) VALUES ({values[..^2]}) RETURNING {idColumnName}";
        return query;
    }
}
