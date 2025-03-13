using System.Linq.Expressions;

namespace BlinkDatabase.General;

/// <summary>
/// Exposes methods to easily retrieve, insert, update or delete entites from the database table, using given entity type. 
/// </summary>
/// <typeparam name="T">Type of entity class, which is marked properly with all requied attributes. Must be class and has parameterless constructor.</typeparam>
public interface IRepository<T> where T : class, new()
{
    /// <summary>
    /// Selects all rows from the database, maps them as entity classes and returns all of them.
    /// </summary>
    IEnumerable<T> Select();

    /// <summary>
    /// Selects rows from the database, based on the given condition, maps them as entity classes and returns all of them.
    /// </summary>
    IEnumerable<T> Select(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Selects only first entity from the database, based on the given condition, maps it as entity class and returns it.
    /// </summary>
    T? SelectSingle(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Checks if entity exists, based on given condition in the database. Returns true if entity exists, otherwise returns false.
    /// </summary>
    bool Exists(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Checks if entity with given ID exists in the database. Returns true if entity exists, otherwise returns false.
    /// </summary>
    bool Exists(int id);

    /// <summary>
    /// Inserts given entity to the database table. Returns number of inserted entities - 0 or 1.
    /// </summary>
    int Insert(T obj);

    /// <summary>
    /// Updates given entity, based on its ID property (property marked with <seealso cref="BlinkDatabase.Annotations.IdAttribute"/>). Returns number of updated entities - 0 or 1.
    /// </summary>
    /// <remarks>If the entity with this ID does not exist in the database, this method will throw exception.</remarks>
    int Update(T obj);

    /// <summary>
    /// Deletes all entities from the database table, based on given condition, and returns number of deleted entities.
    /// </summary>
    int Delete(Expression<Func<T, bool>> expression);
}
