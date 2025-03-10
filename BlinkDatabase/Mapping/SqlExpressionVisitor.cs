using BlinkDatabase.Annotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BlinkDatabase.Mapping;

public class SqlExpressionVisitor<T> : ExpressionVisitor where T : class, new()
{
    private readonly StringBuilder builder = new StringBuilder();
    private Type? enumType;

    public string Condition => builder.ToString();

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);
        builder.Append($" {GetSqlOperator(node.NodeType)} ");
        Visit(node.Right);
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        (string tableName, string columnName) = GetTableAndColumnNames(node);

        if (node.Member.DeclaringType == typeof(DateTime))
        {
            builder.Append(GetQueryForDate(node, tableName, columnName));
        }
        else
        {
            builder.Append($"\"{tableName}\".\"{columnName}\"");
        }

        return node;
    }

    private static string GetQueryForDate(MemberExpression node, string tableName, string columnName) => node.Member.Name switch
    {
        "Year" => $"EXTRACT(YEAR FROM \"{tableName}\".\"{columnName}\")",
        "Month" => $"EXTRACT(MONTH FROM \"{tableName}\".\"{columnName}\")",
        "Day" => $"EXTRACT(DAY FROM \"{tableName}\".\"{columnName}\")",
        "Hour" => $"EXTRACT(HOUR FROM \"{tableName}\".\"{columnName}\")",
        "Minute" => $"EXTRACT(MINUTE FROM \"{tableName}\".\"{columnName}\")",
        "Second" => $"EXTRACT(SECOND FROM \"{tableName}\".\"{columnName}\")",
        "DayOfWeek" => $"EXTRACT(DOW FROM \"{tableName}\".\"{columnName}\")",
        _ => throw new NotSupportedException("You cannot compare date by this property.")
    };

    private (string, string) GetTableAndColumnNames(MemberExpression node)
    {
        MemberExpression tableDefiner = node;
        MemberInfo? columnMember = null;
        string? columnName = null;

        while (tableDefiner.Expression is MemberExpression mem)
        {
            if (tableDefiner.Member.GetCustomAttribute<ColumnAttribute>() != null && columnName == null)
            {
                columnMember = tableDefiner.Member;
            }

            tableDefiner = mem;

            if (tableDefiner.Member.GetCustomAttribute<TableAttribute>() != null)
            {
                break;
            }
        }

        string tableName;

        if ((tableDefiner.Member as PropertyInfo)!.PropertyType.GetCustomAttribute<TableAttribute>() != null)
        {
            tableName = (tableDefiner.Member as PropertyInfo)!.PropertyType.GetCustomAttribute<TableAttribute>()!.TableName;
        }
        else
        {
            tableName = (tableDefiner.Member as PropertyInfo)!.DeclaringType!.GetCustomAttribute<TableAttribute>()!.TableName;
        }

        columnMember ??= node.Member;
        enumType = (tableDefiner.Member as PropertyInfo)!.PropertyType.IsEnum ? (tableDefiner.Member as PropertyInfo)!.PropertyType : null;

        return (tableName, columnMember.GetCustomAttribute<ColumnAttribute>()!.ColumnName);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Type == typeof(string))
        {
            builder.Append($"'{node.Value}'");
        }
        else if (enumType != null)
        {
            builder.Append($"'{EnumValueAttribute.GetEnumValueName(enumType, node.Value!)}'");
        }
        else
        {
            builder.Append(node.Value);
        }

        return node;
    }

    private static string GetSqlOperator(ExpressionType nodeType) => nodeType switch
    {
        ExpressionType.Equal => "=",
        ExpressionType.NotEqual => "<>",
        ExpressionType.GreaterThan => ">",
        ExpressionType.LessThan => "<",
        ExpressionType.GreaterThanOrEqual => ">=",
        ExpressionType.LessThanOrEqual => "<=",
        ExpressionType.AndAlso => "AND",
        ExpressionType.OrElse => "OR",
        _ => throw new NotSupportedException($"Operator {nodeType} is not supported.")
    };
}
