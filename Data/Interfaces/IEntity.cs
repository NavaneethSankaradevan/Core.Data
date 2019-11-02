using System;

namespace Core.Data
{
    /// <summary>
    /// Base Entity Model.
    /// </summary>
    public interface IEntity<T>
        where T : struct, IEquatable<T>, IComparable<T>
    {

        /// <summary>
        /// Primary key of Entity.
        /// </summary>
        T ID { get; set; }

        /// <summary>
        /// Whether the entity is in-active.
        /// </summary>
        bool InActive { get; set; }

        /// <summary>
        /// User who made the entity as inactive.
        /// </summary>
        string InActiveByUser { get; set; }

        /// <summary>
        /// Handle concurrency updation. Updated automatically.
        /// </summary>        
        byte[] RowVersion { get; set; }

        /// <summary>
        /// Check the Id is null.
        /// </summary> 
        bool IsNullOrEmpty(T t);

        /// <summary>
        /// Compare the ID with another value.
        /// </summary> 
        bool IsEqual(T t);
    }
}
