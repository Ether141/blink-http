using BlinkDatabase.Annotations;
using BlinkDatabase.Mapping;
using System.Linq.Expressions;
using System.Text;

namespace BlinkDatabase.General;

internal class SqlSelectBuilder
{
    internal static string SelectAll<T>() where T : class, new() => SelectInternal<T>(null);

    internal static string SelectWhere<T>(Expression<Func<T, bool>> expression) where T : class, new() => SelectInternal<T>(SqlWhereBuilder.Where(expression));

    internal static string SelectExist<T>(Expression<Func<T, bool>> expression) where T : class, new()
    {
        string query = SelectInternal<T>(SqlWhereBuilder.Where(expression));
        return "SELECT 1 " + query[query.IndexOf("FROM")..] + " LIMIT 1";
    }

    internal static string SelectExist<T>(object id) where T : class, new() =>
        $"SELECT 1 FROM {TableAttribute.GetTableName<T>()} WHERE \"{ObjectProperty.GetIdProperty(typeof(T)).ColumnName}\" = {ObjectProperty.GetValueAsSqlString(id)} LIMIT 1";

    internal static string SelectInternal<T>(string? whereQuery) where T : class, new()
    {
        string tableName = TableAttribute.GetTableName<T>();
        (string? leftJoin, string? leftJoinValues) = SqlLeftJoinBuilder.LeftJoin<T>();

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
            columnNames += leftJoinValues[..^2];
        }
        else
        {
            columnNames = columnNames[..^2];
        }

        ObjectProperty idProperty = properties.First(p => p.IsId);

        if (leftJoin != null)
        {
            leftJoin = OptimizeLeftJoin(leftJoin);
            leftJoin = " " + leftJoin;
        }

        columnNames = OptimizeColumnNames(columnNames);

        return $"SELECT {columnNames} FROM \"{tableName}\"" + (leftJoin ?? "") + (whereQuery ?? "");
    }

    private static string OptimizeColumnNames(string columnNames) => string.Join(", ", columnNames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToHashSet());

    private static string OptimizeLeftJoin(string leftJoin)
    {
        List<string> parts = leftJoin.Split("LEFT JOIN", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(p => $"LEFT JOIN {p}").ToList();
        List<string> tableNames = [];
        StringBuilder builder = new StringBuilder();

        foreach (string p in parts)
        {
            string tableName = p[10..(p.IndexOf('"', 11) + 1)];

            if (tableNames.Contains(tableName))
            {
                continue;
            }

            tableNames.Add(tableName);
            builder.Append(p + " ");
        }

        return builder.ToString()[..^1];
    }
}
