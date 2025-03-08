using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;
using System.Linq.Expressions;
using System.Reflection;

namespace BlinkDatabase.Sql;

public class SqlSelectBuilder
{
    public static string SelectAll<T>() where T : class, new() => SelectInternal<T>(null);

    public static string SelectWhere<T>(Expression<Func<T, bool>> expression) where T : class, new() => SelectInternal<T>(SqlWhereBuilder.Where(expression));

    internal static string SelectInternal<T>(string? whereQuery) where T : class, new()
    {
        string tableName = typeof(T).GetCustomAttribute<TableAttribute>()!.TableName;
        (string? innerJoin, string? innerJoinValues) = GetInnerJoin<T>();

        if (innerJoin != null)
        {
            innerJoin = " " + innerJoin;
        }

        if (whereQuery != null)
        {
            whereQuery = " " + whereQuery;
        }

        string columnNames = "";

        ObjectProperty[] properties = ObjectProperty.GetProperties<T>();

        foreach (ObjectProperty prop in properties)
        {
            columnNames += $"\"{tableName}\".\"{prop.ColumnName}\" AS \"{tableName}.{prop.ColumnName}\", ";
        }

        if (innerJoinValues != null)
        {
            columnNames += innerJoinValues;
        }
        else
        {
            columnNames = columnNames[..^2];
        }

        return $"SELECT {columnNames} FROM \"{tableName}\"" + (innerJoin ?? "") + (whereQuery ?? "");
    }

    private static (string?, string?) GetInnerJoin<T>() where T : class, new() => GetInnerJoin(typeof(T));

    private static (string?, string?) GetInnerJoin(Type type, Type? exceptType = null)
    {
        string tableName = type.GetCustomAttribute<TableAttribute>()!.TableName;
        ObjectProperty[] relationProperties = [.. ObjectProperty.GetProperties(type).Where(p => p.IsRelation)];

        string? innerJoin = null;
        string? fieldNames = null;

        if (relationProperties.Length > 0)
        {
            innerJoin = "INNER JOIN";
            fieldNames = "";

            foreach (ObjectProperty relationProperty in relationProperties)
            {
                if (exceptType == relationProperty.StoredType)
                {
                    continue;
                }

                innerJoin += $" \"{relationProperty.RelationTableName}\" ON \"{tableName}\".\"{relationProperty.ColumnName}\" = \"{relationProperty.RelationTableName}\".\"{relationProperty.RelationColumnName}\"";
                innerJoin += " INNER JOIN";

                ObjectProperty[] properties = ObjectProperty.GetProperties(relationProperty.StoredType);

                foreach (ObjectProperty prop in properties)
                {
                    fieldNames += $"\"{relationProperty.RelationTableName}\".\"{prop.ColumnName}\" AS \"{relationProperty.RelationTableName}.{prop.ColumnName}\", ";
                }

                innerJoin = innerJoin[..^11];
                fieldNames = fieldNames[..^2];

                (string? nextInnerJoin, string? nextFieldNames) = GetInnerJoin(relationProperty.StoredType, type);

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
