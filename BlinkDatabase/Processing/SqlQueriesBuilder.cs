using BlinkDatabase.General;
using BlinkDatabase.Mapping;
using System.Linq.Expressions;

namespace BlinkDatabase.Processing;

/// <summary>
/// Provides methods to build SQL queries for various database operations.
/// </summary>
public static class SqlQueriesBuilder
{
    /// <summary>
    /// Generates a SQL query to select all records of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the entity to select.</typeparam>
    /// <returns>A SQL query string to select all records.</returns>
    public static string SelectAll<T>() where T : class, new() => SqlSelectBuilder.SelectAll<T>();

    /// <summary>
    /// Generates a SQL query to select records of the specified type that match the given condition.
    /// </summary>
    /// <typeparam name="T">The type of the entity to select.</typeparam>
    /// <param name="expression">The condition to filter the records.</param>
    /// <returns>A SQL query string to select records matching the condition.</returns>
    public static string SelectWhere<T>(Expression<Func<T, bool>> expression) where T : class, new() => SqlSelectBuilder.SelectWhere<T>(expression);

    /// <summary>
    /// Generates a SQL query to check if any records of the specified type match the given condition.
    /// </summary>
    /// <typeparam name="T">The type of the entity to check.</typeparam>
    /// <param name="expression">The condition to filter the records.</param>
    /// <returns>A SQL query string to check for the existence of matching records.</returns>
    public static string SelectExist<T>(Expression<Func<T, bool>> expression) where T : class, new() => SqlSelectBuilder.SelectExist<T>(expression);

    /// <summary>
    /// Generates a SQL query to check if a record of the specified type exists with the given ID.
    /// </summary>
    /// <typeparam name="T">The type of the entity to check.</typeparam>
    /// <param name="id">The ID of the record to check.</param>
    /// <returns>A SQL query string to check for the existence of a record with the given ID.</returns>
    public static string SelectExist<T>(object id) where T : class, new() => SqlSelectBuilder.SelectExist<T>(id);

    /// <summary>
    /// Generates a SQL query to insert a new record of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the entity to insert.</typeparam>
    /// <param name="obj">The object representing the record to insert.</param>
    /// <returns>A SQL query string to insert the record.</returns>
    public static string Insert<T>(T obj) where T : class, new() => SqlInsertBuilder.Insert<T>(obj);

    /// <summary>
    /// Generates a SQL query to update an existing record of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the entity to update.</typeparam>
    /// <param name="obj">The object representing the record to update.</param>
    /// <returns>A SQL query string to update the record.</returns>
    public static string Update<T>(T obj) where T : class, new() => SqlUpdateBuilder.Update<T>(obj);

    /// <summary>
    /// Generates a SQL query to delete records of the specified type that match the given condition.
    /// </summary>
    /// <typeparam name="T">The type of the entity to delete.</typeparam>
    /// <param name="expression">The condition to filter the records to delete.</param>
    /// <returns>A SQL query string to delete the matching records.</returns>
    public static string Delete<T>(Expression<Func<T, bool>> expression) where T : class, new() => SqlDeleteBuilder.Delete(expression);
}
