using BlinkDatabase.Annotations;
using System.Linq.Expressions;

namespace BlinkDatabase.General;

internal static class SqlDeleteBuilder
{
    internal static string Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
    {
        string tableName = TableAttribute.GetTableName<T>();
        string query = $"DELETE FROM \"{tableName}\" " + SqlWhereBuilder.Where(expression);
        return query;
    }
}
