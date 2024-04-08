using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QWiz.Helpers.Paginator;

namespace QWiz.Helpers.Extensions;

public static class QueryExtension
{
    public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query,
        params Expression<Func<T, object>>[]? includes)
        where T : class
    {
        if (includes != null) query = includes.Aggregate(query, EvaluateInclude);

        return query;
    }

    private static IQueryable<T> EvaluateInclude<T>(IQueryable<T> current, Expression<Func<T, object>> item)
        where T : class
    {
        if (item.Body is not MethodCallExpression expression) return current.Include(item);
        var arguments = expression.Arguments;
        if (arguments.Count <= 1) return current.Include(item);
        var navigationPath = string.Empty;
        for (var i = 0; i < arguments.Count; i++)
        {
            var arg = arguments[i];
            var path = arg.ToString()[(arg.ToString().IndexOf('.') + 1)..];

            navigationPath += (i > 0 ? "." : string.Empty) + path;
        }

        return current.Include(navigationPath);
    }

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string attribute, Order direction = Order.Asc)
    {
        return ApplyOrdering(query, "OrderBy", attribute, direction);
    }

    public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string attribute,
        Order direction = Order.Asc)
    {
        return ApplyOrdering(query, "ThenBy", attribute, direction);
    }

    private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query,
        string orderMethodName, string attribute, Order direction = Order.Asc)
    {
        try
        {
            if (direction == Order.Desc) orderMethodName += "Descending";

            var t = typeof(T);

            var param = Expression.Parameter(t);

            var property = t.GetProperty(attribute.ToTitleCase());

            if (property == null)
                throw new System.Exception($"{attribute} property not found in table {typeof(T).Name}");

            return query.Provider.CreateQuery<T>(
                Expression.Call(
                    typeof(Queryable),
                    orderMethodName,
                    new[] { t, property.PropertyType },
                    query.Expression,
                    Expression.Quote(
                        Expression.Lambda(
                            Expression.Property(param, property),
                            param))
                ));
        }
        catch (System.Exception)
        {
            return query;
        }
    }
}