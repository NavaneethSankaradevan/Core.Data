using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Data
{
    /// <summary>
    /// Handler for manage the Commits.
    /// </summary>
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {

        #region Fields
        private readonly DbContext _dbContext;

        private readonly ILogger _logger;
        #endregion

        /// <summary>
        /// Initialize.
        /// </summary>
        public UnitOfWork(DbContext context)
        {
            _dbContext = context;
            _logger = new LoggerFactory().CreateLogger("UnitOfWork"); ;
        }

        DbContext IUnitOfWork.DataContext
        {
            get
            {
                return _dbContext;
            }
        }

        async Task<int> IUnitOfWork.CommitAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while Committing. Error: {ex}");
                throw ex;
            }
        }

        void IDisposable.Dispose() => _dbContext.Dispose();

    }
}
