using BlinkDatabase.Mapping;
using System.Linq.Expressions;

namespace BlinkDatabase.General;

internal class SqlWhereBuilder
{
    internal static string Where<T>(Expression<Func<T, bool>> expression) where T : class, new() => Where<T>(expression, false);

    internal static string Where<T>(Expression<Func<T, bool>> expression, bool skipRelations) where T : class, new()
    {
        var visitor = new SqlExpressionVisitor<T>(skipRelations);
        visitor.Visit(expression.Body);
        return "WHERE " + visitor.Condition;
    }
}
