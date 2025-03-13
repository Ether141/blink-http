using BlinkDatabase.Mapping;
using System.Linq.Expressions;

namespace BlinkDatabase.General;

internal class SqlWhereBuilder
{
    internal static string Where<T>(Expression<Func<T, bool>> expression) where T : class, new()
    {
        var visitor = new SqlExpressionVisitor<T>();
        visitor.Visit(expression.Body);
        return "WHERE " + visitor.Condition;
    }
}
