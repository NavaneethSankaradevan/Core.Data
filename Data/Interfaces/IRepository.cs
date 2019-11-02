using System;

namespace Core.Data
{
    /// <summary>
    /// Base Repository with CRUD operations.
    /// </summary>
    public interface IRepository<tEntity, tType> :
        IReadRepository<tEntity, tType>,
        IWriteRepository<tEntity, tType>
           where tEntity : class, IEntity<tType>
           where tType : struct, IEquatable<tType>, IComparable<tType>
    { }
}
