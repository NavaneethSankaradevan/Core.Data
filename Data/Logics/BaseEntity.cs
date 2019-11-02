
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Data
{

    /// <summary>
    /// Base class for entity class.
    /// </summary>
    public abstract class BaseEntity<T> : IEntity<T>
        where T : struct, IEquatable<T>, IComparable<T>
    {

        /// <summary>
        /// Primary key field of the object.
        /// </summary>
        [Key]
        public T ID { get; set; }

        /// <summary>
        /// Field used to indicate whether the entity is active or not.
        /// </summary>
        public bool InActive { get; set; }

        /// <summary>
        /// User who made the entity as In-active.
        /// </summary>
        public string InActiveByUser { get; set; }

        /// <summary>
        /// Handle concurrency updation. Updated automatically.
        /// </summary>
        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }

        /// <summary>
        /// Check the Id is null.
        /// </summary>
        bool IEntity<T>.IsNullOrEmpty(T t)
        {
            return t.Equals(null) || t.Equals(default(T));
        }

        /// <summary>
        /// Check the Id is equal to given value.
        /// </summary>
        bool IEntity<T>.IsEqual(T t)
        {
            return this.ID.Equals(t);
        }

    }
}
