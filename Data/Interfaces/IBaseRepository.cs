using System;

namespace MSF.Core
{
    /// <summary>
    /// Base Repository with CRUD operations.
    /// </summary>
    public interface IBaseRepository<tEntity, tType> :
        IReadRepository<tEntity, tType>,
        IWriteRepository<tEntity, tType>
           where tEntity : class, IBaseEntity<tType>
           where tType : struct, IEquatable<tType>, IComparable<tType>
    { }
}
