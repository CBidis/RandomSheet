using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Timesheet.Business.Dtos;
using Timesheets.Domain.Models;

namespace Timesheet.Business.Contracts
{
    /// <summary>
    /// Generic Service Contract Definition
    /// </summary>
    /// <typeparam name="TEntity">Derived type of Entity Base (aka Domain Entity)</typeparam>
    /// <typeparam name="TKey">Type parameter of Key Value</typeparam>
    /// <typeparam name="TDto">Derived Type of BaseDto</typeparam>
    public interface IGenericService<TEntity, TKey, TDto> where TEntity : class, IEntity<TKey> where TDto : IDto<TKey> where TKey : struct
    {
        /// <summary>
        /// Paginated Results
        /// </summary>
        /// <param name="filter">filter value</param>
        /// <param name="orderColumn">order column</param>
        /// <param name="skipSize">skip size</param>
        /// <param name="takeSize">page size</param>
        /// <param name="loadRefs">Includable References for Joins</param>
        /// <returns></returns>
        Task<(IEnumerable<TDto>, int totalRecords)> PaginatedResultsAsync(string filter, string orderColumn, int skipSize, int takeSize, params string[] loadRefs);
        /// <summary>
        /// Filtered Results given a where Predicate
        /// </summary>
        /// <param name="predicate">where predicate</param>
        /// <param name="loadRefs">load referenced entities</param>
        /// <returns></returns>
        Task<List<TDto>> FilterBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> selector = null, params string[] loadRefs);
        /// <summary>
        /// Persists the new object to Database
        /// </summary>
        /// <param name="dto">dto object</param>
        /// <returns>created object primary key value</returns>
        Task<TKey> CreateAsync(TDto dto);
        /// <summary>
        /// Deletes the object from Database
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns>number of affected rows</returns>
        Task<int> DeleteAsync(TKey keyValue);
        /// <summary>
        /// Persist the changes of the object to Database
        /// </summary>
        /// <param name="dto">dto object</param>
        /// <typeparam name="TDto">Derived Type of BaseDto</typeparam>
        /// <returns>number of affected rows</returns>
        Task<int> UpdateAsync(TDto dto, TKey keyValue);
        /// <summary>
        /// Gets Objects by key value
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="loadRefs">load referenced entities</param>
        /// <typeparam name="TDto">Derived Type of BaseDto</typeparam>
        /// <returns></returns>
        Task<TDto> FindById(TKey keyValue, params string[] loadRefs);
    }
}
