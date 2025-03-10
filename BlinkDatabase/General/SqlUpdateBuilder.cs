using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;

namespace BlinkDatabase.General;

public static class SqlUpdateBuilder
{
    public static string Update<T>(T obj) where T : class, new()
    {
        ObjectProperty[] properties = [.. ObjectProperty.GetProperties<T>().Where(p => !p.IsId)];
        string tableName = TableAttribute.GetTableName<T>();
        string values = "";

        foreach (ObjectProperty prop in properties)
        {
            if (prop.RelationType == RelationType.OneToMany)
            {
                continue;
            }

            if (prop.Get(obj) == null)
            {
                continue;
            }

            string value = $"\"{prop.ColumnName}\" = ";

            if (!prop.IsRelation)
            {
                value += $"{prop.GetAsSqlString(obj)!}, ";
            }
            else
            {
                object objFromRelation = prop.Get(obj)!;
                object relationId = ObjectProperty.GetIdProperty(objFromRelation.GetType()).Get(objFromRelation)!;
                value += $"{relationId}, ";
            }

            values += value;
        }

        ObjectProperty idProperty = ObjectProperty.GetIdProperty(typeof(T));
        string query = $"UPDATE \"{tableName}\" SET {values[..^2]} WHERE \"{idProperty.ColumnName}\" = {idProperty.Get(obj)}";
        return query;
    }
}
