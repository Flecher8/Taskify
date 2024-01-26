﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Taskify.DAL.Repositories
{
    public abstract class BaseFilterableRepository<T, TFilterBuilder> : BaseRepository<T>
     where T : class
     where TFilterBuilder : new()
    {
        protected readonly TFilterBuilder _filterBuilder;

        public BaseFilterableRepository(DataContext dbContext) : base(dbContext)
        {
            _filterBuilder = new TFilterBuilder();
        }

        public virtual async Task<List<T>> GetFilteredItemsAsync(Func<IQueryable<T>, IQueryable<T>> includeEntities, Action<TFilterBuilder> buildFilter)
        {
            buildFilter(_filterBuilder);

            var query = _dbContext.Set<T>().AsQueryable();
            query = includeEntities(query);

            return await query
                .Where((Expression<Func<T, bool>>)_filterBuilder.GetType().GetProperty("Filter").GetValue(_filterBuilder))
                .ToListAsync();
        }
    }
}
