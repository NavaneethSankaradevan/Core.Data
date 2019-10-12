using System;

namespace MSF.Core
{

    /// <summary>
    /// Base class for transaction entity class.
    /// </summary>
    public abstract class BaseTransactionalEntity<T> : BaseEntity<T>, IBaseEntity<T>
        where T : struct, IEquatable<T>, IComparable<T>
    {

        /// <summary>
        /// Created by user. This is string type since the default Id type of Micorsoft's IdentityUser is string.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Created date, Automatically updated.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Updated by user. This is string type since the default Id type of Micorsoft's IdentityUser is string.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Last Updated date, Automatically updated.
        /// </summary>
        public DateTime UpdatedOn { get; set; }

    }
}
