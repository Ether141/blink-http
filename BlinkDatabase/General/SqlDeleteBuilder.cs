using BlinkDatabase.Annotations;
using System.Linq.Expressions;

namespace BlinkDatabase.General;

public static class SqlDeleteBuilder
{
    public static string Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
    {
        string tableName = TableAttribute.GetTableName<T>();
        string query = $"DELETE FROM \"{tableName}\" " + SqlWhereBuilder.Where(expression);
        return query;
    }
}
