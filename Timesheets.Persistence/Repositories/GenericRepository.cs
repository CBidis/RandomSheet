using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Timesheets.Domain.Models;
using Timesheets.Persistence.Contracts;
using Timesheets.Persistence.Exceptions;
using Timesheets.Persistence.Extensions;

namespace Timesheets.Persistence.Repositories
{
    /// <summary>
    /// Generic Implementation of IRepository Interface, Derive and Override to customize it even further.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class GenericRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey> where TKey : struct
    {
        private readonly TimesheetDbContext _dbContext;

        public GenericRepository(TimesheetDbContext dbContext) => _dbContext = dbContext;

        public virtual IQueryable<TEntity> AllQuerable(params string[] loadRefs)
        {
            IQueryable<TEntity> entityContext = _dbContext.Set<TEntity>().AsNoTracking();

            if (loadRefs != null && loadRefs.Any())
                entityContext = loadRefs.Aggregate(entityContext, (current, inc) => current.Include(inc));

            return entityContext;
        }

        public async Task<(IEnumerable<TEntity>, int totalRecords)> PaginatedResultsAsync(Expression<Func<TEntity, bool>> whereFunc,
                                                                        Expression<Func<TEntity, TEntity>> selector, string orderColumn, int skipSize, int takeSize, params string[] loadRefs)
        {
            IQueryable<TEntity> entityQuerable = AllQuerable(loadRefs);

            if (whereFunc != null)
                entityQuerable = entityQuerable.Where(whereFunc);


            var totalRecords = await entityQuerable.CountAsync();

            if (selector != null)
                entityQuerable = entityQuerable.Select(selector);

            entityQuerable = entityQuerable.Paging<TEntity, TKey>(orderColumn, skipSize, takeSize);

            return totalRecords == 0 ?
                (new List<TEntity>(), totalRecords) : (entityQuerable.ToList(), totalRecords);
        }

        public async virtual Task<TEntity> FindByIdAsync(TKey key, params string[] loadRefs)
        {
            IQueryable<TEntity> entityContext = _dbContext.Set<TEntity>();

            if (loadRefs != null && loadRefs.Any())
                entityContext = loadRefs.Aggregate(entityContext, (current, inc) => current.Include(inc));

            TEntity entityRow = await entityContext.FirstOrDefaultAsync(p => p.Id.Equals(key));

            if (entityRow == null)
                throw new EntityNotFoundException(typeof(TEntity).Name, key, $"For Table {typeof(TEntity).Name} there is no row with id {key}");

            return entityRow;
        }

        public virtual void Add(TEntity entity) => _dbContext.Add(entity);

        public void AddRange(IEnumerable<TEntity> entities) => _dbContext.AddRange(entities);

        public async virtual Task AddAsync(TEntity entity) => await _dbContext.AddAsync(entity);

        public async virtual Task AddRangeAsync(IEnumerable<TEntity> entities) => await _dbContext.AddRangeAsync(entities);

        public virtual void Remove(TEntity entity) => _dbContext.Remove(entity);

        public virtual void RemoveRange(IEnumerable<TEntity> entities) => _dbContext.RemoveRange(entities);

        public virtual void Update(TEntity entity) => _dbContext.Update(entity);

        public virtual void UpdateRange(IEnumerable<TEntity> entities) => _dbContext.UpdateRange(entities);

        public virtual int CommitChanges() => _dbContext.SaveChanges();

        public async virtual Task<int> CommitChangesAsync() => await _dbContext.SaveChangesAsync();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    _dbContext?.Dispose();

                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
