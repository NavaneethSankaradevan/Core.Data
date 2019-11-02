
using System;
using System.Threading.Tasks;

namespace Core.Data
{
    /// <summary>
    /// Base Repository with Write operations.
    /// </summary>
    public interface IWriteRepository<tEntity, tType>
        where tEntity : class, IEntity<tType>
        where tType : struct, IEquatable<tType>, IComparable<tType>
    {

        /// <summary>
        /// Create new entity or Update existing entity.
        /// </summary>
        /// <returns></returns>
        Task<tType> SaveAsync(tEntity entity);

        /// <summary>
        /// Create new entity or Update existing entity with user info.
        /// </summary>
        /// <returns></returns>
        Task<tType> SaveAsync(tEntity entity, string user);

        /// <summary>
        /// Delete the entity from Database.
        /// </summary>
        /// <param name="ID">Entity Id</param>
        /// <returns></returns>
        Task HardDelete(tType ID);

        /// <summary>
        /// Mark the entity is deleted state in Database.
        /// </summary>
        /// <param name="ID">Entity Id</param>
        /// <returns></returns>
        Task SoftDelete(tType ID);

        /// <summary>
        /// Mark the entity is deleted state in Database with user info.
        /// </summary>
        /// <param name="ID">Entity Id</param>
        /// <param name="user">Deleted User </param>
        /// <returns></returns>
        Task SoftDelete(tType ID, string user);

    }
}
