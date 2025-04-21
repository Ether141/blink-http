using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;
using System.Linq.Expressions;

namespace BlinkDatabase.General;

internal class SqlSelectBuilder
{
    internal static string SelectAll<T>() where T : class, new() => SelectInternal<T>(null);

    internal static string SelectWhere<T>(Expression<Func<T, bool>> expression) where T : class, new() => SelectInternal<T>(SqlWhereBuilder.Where(expression));

    internal static string SelectExist<T>(Expression<Func<T, bool>> expression) where T : class, new()
    {
        string query = SelectInternal<T>(SqlWhereBuilder.Where(expression));
        query = "SELECT 1 " + query[query.IndexOf("FROM")..] + " LIMIT 1";
        return query;
    }

    internal static string SelectExist<T>(int id) where T : class, new()
    {
        string query = $"SELECT 1 FROM {TableAttribute.GetTableName<T>()} WHERE \"{ObjectProperty.GetIdProperty(typeof(T)).ColumnName}\" = {id} LIMIT 1";
        return query;
    }

    internal static string SelectInternal<T>(string? whereQuery) where T : class, new()
    {
        string tableName = TableAttribute.GetTableName<T>();
        (string? leftJoin, string? leftJoinValues) = SqlLeftJoinBuilder.LeftJoin<T>();

        if (leftJoin != null)
        {
            leftJoin = " " + leftJoin;
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

        if (leftJoinValues != null)
        {
            columnNames += leftJoinValues;
        }
        else
        {
            columnNames = columnNames[..^2];
        }

        ObjectProperty idProperty = properties.First(p => p.IsId);

        return $"SELECT {columnNames} FROM \"{tableName}\"" + (leftJoin ?? "") + (whereQuery ?? "");
    }
}
