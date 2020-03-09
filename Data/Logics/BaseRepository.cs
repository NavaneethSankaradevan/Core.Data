using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Data
{
    /// <summary>
    /// Base Repository with CRUD operations.
    /// </summary>
    public abstract class BaseRepository<tEntity, tType> : IRepository<tEntity, tType>
        where tEntity : class, IEntity<tType>
        where tType : struct, IEquatable<tType>, IComparable<tType>
    {

        #region Variables
        private DbSet<tEntity> _dbSet = null;

        /// <summary>
        /// Data Context.
        /// </summary>
        protected DbContext DataContext { get; private set; }

        private readonly ILogger _logger = null;

        /// <summary>
        /// Current Entity.
        /// </summary>
        protected IQueryable<tEntity> Entity
        {
            get
            {
                return _dbSet;
            }
        }

        /// <summary>
        /// Is auto commit enabled.
        /// </summary>
        public bool AutoCommitEnabled { get; set; }

        #endregion

        #region Protected Method

        /// <summary>
        /// Base Repository.
        /// </summary>
        protected BaseRepository(IUnitOfWork unitOfWork)
        {
            DataContext = unitOfWork.DataContext;
            _dbSet = DataContext.Set<tEntity>();
            _logger = new LoggerFactory().CreateLogger("Repository");
        }

        /// <summary>
        /// Virtual method to override the entity with include the dependencies.
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<tEntity> GetEntitySet(bool incluedeDeleted = false)
        {
            _logger.LogInformation($"Get Request triggered for object:{typeof(tEntity).Name}");

            if (!incluedeDeleted)
            {
                _logger.LogInformation("Default query with active items ");
                return Entity.Where(e => !e.InActive);
            }
            else
            {
                _logger.LogInformation("Default query includeing in active ");
                return Entity;
            }
        }

        #endregion

        #region IBaseRepository Implementations

        #region IReadRepository Implementations
        async Task<List<tEntity>> IReadRepository<tEntity, tType>.GetAllAsync()
        {
            try
            {
                return await GetEntitySet().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on GetAll. Error:{ex}");
                throw ex;
            }
        }

        async Task<tEntity> IReadRepository<tEntity, tType>.GetAsync(tType ID)
        {
            try
            {
                return await GetEntitySet().FirstOrDefaultAsync(r => r.ID.Equals(ID));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on Get by Id:{ID}. Error:{ex}");
                throw ex;
            }

        }

        async Task<List<tEntity>> IReadRepository<tEntity, tType>.GetAllPageData<tOrderBy>(
            Expression<Func<tEntity, tOrderBy>> OrderBy, int PageNo, int PageCount)
        {
            try
            {
                return await GetEntitySet().OrderBy(OrderBy)
                    .Skip(PageNo * PageCount).Take(PageCount).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on GetAllPageData for page :{PageNo}. Error:{ex}");
                throw ex;
            }
        }

        async Task<List<tEntity>> IReadRepository<tEntity, tType>.FindByAsync(
            Expression<Func<tEntity, bool>> predicate)
        {
            try
            {
                return await GetEntitySet().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on Find by condition  :{predicate}. Error:{ex}");
                throw ex;
            }
        }

        async Task<List<tEntity>> IReadRepository<tEntity, tType>.FindByWithPagingAsync<tOrderBy>(
            Expression<Func<tEntity, bool>> predicate,
            Expression<Func<tEntity, tOrderBy>> OrderBy,
            int PageNo, int PageCount)
        {
            try
            {
                return await GetEntitySet().Where(predicate).OrderBy(OrderBy)
                    .Skip(PageNo * PageCount).Take(PageCount).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on FindByWithPaging for condition:{predicate}, page :{PageNo}. Error:{ex}");
                throw ex;
            }
        }

        #endregion

        #region IWriteRepository Implementations

        async Task<tType> IWriteRepository<tEntity, tType>.SaveAsync(tEntity entity)
        {
            return await SaveAsync(entity, string.Empty);
        }

        async Task<tType> IWriteRepository<tEntity, tType>.SaveAsync(tEntity entity, string user)
        {
            return await SaveAsync(entity, user);
        }

        async Task IWriteRepository<tEntity, tType>.HardDelete(tType ID)
        {

            _logger.LogInformation(" Hard delete trigger..");

            tEntity entity = await Entity.FirstOrDefaultAsync(e => e.ID.Equals(ID));
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            else
            {
                throw new ArgumentException($"{ID} - Not Found");
            }
        }

        async Task IWriteRepository<tEntity, tType>.SoftDelete(tType ID)
        {
            await SoftDelete(ID, string.Empty);
        }

        async Task IWriteRepository<tEntity, tType>.SoftDelete(tType ID, string user)
        {
            await SoftDelete(ID, user);
        }

        #endregion

        #endregion

        #region Private Method

        private async Task<tType> SaveAsync(tEntity entity, string user)
        {
            // Update last modified by.
            UpdateAuditDetails(entity, user);

            // Check Add or Update by ID field.
            var isExist = await Entity.AnyAsync(e => e.ID.Equals(entity.ID));

            _logger.LogInformation($"{(isExist ? "Update" : "Create")} triggered for:{typeof(tEntity).Name}");

            if (isExist)
            {

                DataContext.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                await _dbSet.AddAsync(entity);
            }
            return entity.ID;
        }

        private async Task SoftDelete(tType ID, string user)
        {
            tEntity entity = await Entity.FirstOrDefaultAsync(e => e.ID.Equals(ID));
            if (entity != null)
            {

                // Update last modified by.
                UpdateAuditDetails(entity, user);

                _logger.LogInformation(" Soft delete trigger..");

                // Set entity is in active.
                entity.InActive = true;
                entity.InActiveByUser = user;
                DataContext.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                throw new ArgumentException($"{ID} - Not Found");
            }
        }

        /// <summary>
        /// Update the DateTime fields for Transaction entities.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user">Modified by</param>
        private void UpdateAuditDetails(tEntity entity, string user)
        {
            if (typeof(BaseEntityTrackable<>).GetGenericTypeDefinition() == typeof(tEntity))
            {
                _logger.LogInformation(" Updating the UpdateOn column");

                (entity as BaseEntityTrackable<tType>).UpdatedOn = DateTime.UtcNow;
                (entity as BaseEntityTrackable<tType>).UpdatedBy = user;

                if (entity.IsNullOrEmpty(entity.ID))
                {
                    (entity as BaseEntityTrackable<tType>).CreatedOn = DateTime.UtcNow;
                    (entity as BaseEntityTrackable<tType>).CreatedBy = user;
                }

            }
        }

        #endregion
    }
}
