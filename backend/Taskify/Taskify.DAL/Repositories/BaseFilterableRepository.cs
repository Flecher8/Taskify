using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.DAL.Interfaces;

namespace Taskify.DAL.Repositories
{
    public abstract class BaseFilterableRepository<T, TFilterBuilder> : BaseRepository<T>, IFilterableRepository<T, TFilterBuilder>
     where T : class
     where TFilterBuilder : new()
    {
        protected readonly TFilterBuilder _filterBuilder;

        public BaseFilterableRepository(DataContext dbContext) : base(dbContext)
        {
            _filterBuilder = new TFilterBuilder();
        }

        public virtual async Task<List<T>> GetFilteredItemsAsync(Action<TFilterBuilder> buildFilter)
        {
            buildFilter(_filterBuilder);

            var query = _dbContext.Set<T>().AsQueryable();
            query = IncludeEntities(query);

            var filterProperty = _filterBuilder.GetType().GetProperty("Filter");
            var filter = filterProperty.GetValue(_filterBuilder) as Expression<Func<T, bool>>;

            // Ensure that the Filter property is initialized
            if (filter == null)
            {
                filter = _ => true; // Default filter if not set
                filterProperty.SetValue(_filterBuilder, filter);
            }

            return await query
                .Where(filter)
                .ToListAsync();
        }

        protected virtual IQueryable<T> IncludeEntities(IQueryable<T> query)
        {
            // Override this method in derived classes to include specific entities
            return query;
        }
    }
}
