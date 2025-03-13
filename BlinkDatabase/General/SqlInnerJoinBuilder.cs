using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;
using System.Reflection;

namespace BlinkDatabase.General;

internal static class SqlInnerJoinBuilder
{
    internal static (string?, string?) InnerJoin<T>() where T : class, new() => InnerJoin(typeof(T));

    internal static (string?, string?) InnerJoin(Type type, Type? exceptType = null)
    {
        string tableName = type.GetCustomAttribute<TableAttribute>()!.TableName;
        ObjectProperty[] relationProperties = [.. ObjectProperty.GetProperties(type).Where(p => p.IsRelation)];

        string? innerJoin = null;
        string? fieldNames = null;

        if (relationProperties.Length > 0)
        {
            innerJoin = "";
            fieldNames = "";

            foreach (ObjectProperty relationProperty in relationProperties)
            {
                if (exceptType == relationProperty.StoredType)
                {
                    continue;
                }

                innerJoin += $"INNER JOIN \"{relationProperty.RelationTableName}\" ON \"{tableName}\".\"{relationProperty.ColumnName}\" = \"{relationProperty.RelationTableName}\".\"{relationProperty.RelationColumnName}\"";

                ObjectProperty[] properties = ObjectProperty.GetProperties(relationProperty.StoredType);

                foreach (ObjectProperty prop in properties)
                {
                    fieldNames += $"\"{relationProperty.RelationTableName}\".\"{prop.ColumnName}\" AS \"{relationProperty.RelationTableName}.{prop.ColumnName}\", ";
                }

                fieldNames = fieldNames[..^2];

                (string? nextInnerJoin, string? nextFieldNames) = InnerJoin(relationProperty.StoredType, type);

                if (innerJoin != null)
                {
                    innerJoin += " " + nextInnerJoin;
                    fieldNames += ", " + nextFieldNames;
                }
            }
        }

        return (innerJoin, fieldNames?.Trim().Trim(','));
    }
}
