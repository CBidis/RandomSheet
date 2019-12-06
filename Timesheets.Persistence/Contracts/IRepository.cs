using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Timesheets.Domain.Models;

namespace Timesheets.Persistence.Contracts
{
    /// <summary>
    /// Repository Methods Definitions upon IEntity Objects
    /// </summary>
    /// <typeparam name="TEntity">Derired Type of EntityBase</typeparam>
    /// <typeparam name="TKey">Type parameter of Key Column</typeparam>
    public interface IRepository<TEntity, TKey> : IDisposable where TEntity : IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// Paginated Results for Entity
        /// </summary>
        /// <param name="whereFunc">where lamda function</param>
        /// <param name="selector">selector for projection</param>
        /// <param name="orderColumn">ordering column</param>
        /// <param name="skipSize">skip size</param>
        /// <param name="takeSize">take size</param>
        /// <param name="loadRefs">includable loading references</param>
        /// <returns>a collection of filtered results and the total count of results</returns>
        Task<(IEnumerable<TEntity>, int totalRecords)> PaginatedResultsAsync(Expression<Func<TEntity, bool>> whereFunc,
                                                                                Expression<Func<TEntity, TEntity>> selector, string orderColumn, int skipSize, int takeSize, params string[] loadRefs);
        /// <summary>
        /// Get Querable Entities
        /// </summary>
        /// <param name="loadRefs">load referenced entities</param>
        /// <returns>IQueryable entities</returns>
        IQueryable<TEntity> AllQuerable(params string[] loadRefs);
        /// <summary>
        /// Get Entity by Key value, asynchronously
        /// </summary>
        /// <param name="key">key value</param>
        /// <param name="loadRefs">load referenced entities</param>
        /// <returns>TEntity object</returns>
        Task<TEntity> FindByIdAsync(TKey key, params string[] loadRefs);
        /// <summary>
        /// Adds Entity to DB Context
        /// </summary>
        /// <param name="entity">entity object</param>
        void Add(TEntity entity);
        /// <summary>
        /// Adds a collection of entities to DB Context
        /// </summary>
        /// <param name="entity">entity object</param>
        void AddRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// Adds Entity to DB Context, asynchronously
        /// </summary>
        /// <param name="entity">entity object</param>
        Task AddAsync(TEntity entity);
        /// <summary>
        /// Adds a collection of entities to DB Context, asynchronously
        /// </summary>
        /// <param name="entity">entity object</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// update and entity of DB Context
        /// </summary>
        /// <param name="entity">entity object</param>
        void Update(TEntity entity);
        /// <summary>
        /// Updates a collection of entities to DB Context
        /// </summary>
        /// <param name="entity">entity object</param>
        void UpdateRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// Remove an object from DB Context
        /// </summary>
        /// <param name="entity">entity object</param>
        void Remove(TEntity entity);
        /// <summary>
        /// Removes a collection of entities to DB Context
        /// </summary>
        /// <param name="entity">entity object</param>
        void RemoveRange(IEnumerable<TEntity> entities);
        /// <summary>
        /// Persist changes to database
        /// </summary>
        /// <returns>number of affected rows</returns>
        int CommitChanges();
        /// <summary>
        /// Persist changes to database, asynchronously
        /// </summary>
        /// <returns>number of affected rows</returns>
        Task<int> CommitChangesAsync();
    }
}
