using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MSF.Core
{
    /// <summary>
    /// Manage commit for each entity.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
		/// Commit the current changes in database.
		/// </summary>
		/// <returns></returns>
		Task<int> CommitAsync();

        /// <summary>
        /// Database context object.
        /// </summary>
        DbContext DataContext { get; }
    }
}
