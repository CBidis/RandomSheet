using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Timesheet.Business.Contracts;
using Timesheets.Domain.Models;
using Timesheet.Business.Dtos;
using Timesheets.Persistence.Repositories;

namespace Timesheet.Business.Services
{
    public class GenericService<TEntity, TKey, TDto> : IGenericService<TEntity, TKey, TDto>
                                                            where TEntity : class ,IEntity<TKey> where TDto : IDto<TKey> where TKey : struct
    {
        protected readonly GenericRepository<TEntity, TKey> _baseRepo;
        protected readonly IMapper _mapper;

        public GenericService(GenericRepository<TEntity, TKey> baseRepo, IMapper mapper)
        {
            _baseRepo = baseRepo;
            _mapper = mapper;
        }

        public virtual async Task<TKey> CreateAsync(TDto dto)
        {
            TEntity entity = _mapper.Map<TEntity>(dto);
            await _baseRepo.AddAsync(entity);
            await _baseRepo.CommitChangesAsync();
            return entity.Id;
        }

        public virtual async Task<int> DeleteAsync(TKey keyValue)
        {
            TEntity entity = await _baseRepo.FindByIdAsync(keyValue);
            _baseRepo.Remove(entity);
            return await _baseRepo.CommitChangesAsync();
        }

        public virtual async Task<List<TDto>> FilterBy(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> selector = null, params string[] loadRefs)
        {
            IQueryable<TEntity> query = _baseRepo.AllQuerable(loadRefs);

            if (predicate != null)
                query = query.Where(predicate);

            if (selector != null)
                return _mapper.Map<List<TDto>>(query.Select(selector));

            List<TEntity> results = await query.ToListAsync();
            return _mapper.Map<List<TDto>>(results);
        }
        public virtual async Task<TDto> FindById(TKey keyValue, params string[] loadRefs)
        {
            TEntity entity = await _baseRepo.FindByIdAsync(keyValue, loadRefs);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<(IEnumerable<TDto>, int totalRecords)> PaginatedResultsAsync(string filter, string orderColumn, int skipSize, int takeSize, params string[] loadRefs)
        {
            (IEnumerable<TEntity> results, var totalRecords) = await _baseRepo.PaginatedResultsAsync(null, null, orderColumn, skipSize, takeSize, loadRefs);
            return (_mapper.Map<List<TDto>>(results), totalRecords);
        }

        public virtual async Task<int> UpdateAsync(TDto dto, TKey keyValue)
        {
            TEntity entity = await _baseRepo.FindByIdAsync(keyValue);
            entity = _mapper.Map<TDto, TEntity>(dto, entity);
            _baseRepo.Update(entity);
            return await _baseRepo.CommitChangesAsync();
        }
    }
}
