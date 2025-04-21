using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;
using System.Reflection;

namespace BlinkDatabase.General;

internal static class SqlLeftJoinBuilder
{
    internal static (string?, string?) LeftJoin<T>() where T : class, new() => LeftJoin(typeof(T));

    internal static (string?, string?) LeftJoin(Type type, Type? exceptType = null)
    {
        string tableName = type.GetCustomAttribute<TableAttribute>()!.TableName;
        ObjectProperty[] relationProperties = [.. ObjectProperty.GetProperties(type).Where(p => p.IsRelation)];

        string? leftJoin = null;
        string? fieldNames = null;

        if (relationProperties.Length > 0)
        {
            leftJoin = "";
            fieldNames = "";

            foreach (ObjectProperty relationProperty in relationProperties)
            {
                if (exceptType == relationProperty.StoredType)
                {
                    continue;
                }

                leftJoin += $"LEFT JOIN \"{relationProperty.RelationTableName}\" ON \"{tableName}\".\"{relationProperty.ColumnName}\" = \"{relationProperty.RelationTableName}\".\"{relationProperty.RelationColumnName}\"";

                ObjectProperty[] properties = ObjectProperty.GetProperties(relationProperty.StoredType);

                foreach (ObjectProperty prop in properties)
                {
                    fieldNames += $"\"{relationProperty.RelationTableName}\".\"{prop.ColumnName}\" AS \"{relationProperty.RelationTableName}.{prop.ColumnName}\", ";
                }

                fieldNames = fieldNames[..^2];

                (string? nextLeftJoin, string? nextFieldNames) = LeftJoin(relationProperty.StoredType, type);

                if (leftJoin != null)
                {
                    leftJoin += " " + nextLeftJoin;
                    fieldNames += ", " + nextFieldNames;
                }
            }
        }

        return (leftJoin, fieldNames?.Trim().Trim(','));
    }
}
