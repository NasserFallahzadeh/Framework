using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace DDDZamin.Utilities.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string sortField, bool ascending)
    {
        var param = Expression.Parameter(typeof(T), "p");

        var prop=Expression.Property(param, sortField);

        var exp = Expression.Lambda(prop, param);

        var method = ascending
            ? "OrderBy"
            : "OrderByDescending";

        var types = new[] { q.ElementType, exp.Body.Type };

        var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);

        return q.Provider.CreateQuery<T>(mce);
    }

    public static DataTable ToDataTable<T>(this List<T> list)
    {
        DataTable dataTable = new(typeof(T).Name);

        var props=typeof(T).GetProperties(BindingFlags.Public|BindingFlags.Instance)
            .ToList();

        props.ForEach(p => dataTable.Columns.Add(p.Name));

        list.ForEach(l =>
        {
            var values = new object[props.Count];

            for (var i = 0; i < props.Count; i++) 
                values[i] = props[i].GetValue(l, null);

            dataTable.Rows.Add(values);
        });

        return dataTable;
    }

    public static List<T> ToList<T>(this DataTable dt) => 
        (from DataRow row in dt.Rows select GetItem<T>(row)).ToList();

    private static T GetItem<T>(DataRow row)
    {
        var genericType=typeof(T);

        var obj = Activator.CreateInstance<T>();

        foreach (DataColumn column in row.Table.Columns)
        {
            var property = genericType.GetProperties()
                .FirstOrDefault(p => p.Name == column.ColumnName);

            property?.SetValue(obj, Convert.ChangeType(row[column.ColumnName], property.PropertyType), null);
        }

        return obj;
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, bool>> predicate) =>
        condition
            ? query.Where(predicate)
            : query;

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if condition is true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, int, bool>> predicate) =>
        condition
            ? query.Where(predicate)
            : query;
}