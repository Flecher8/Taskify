using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.DAL.Repositories
{
    public class CustomTaskRepository : BaseFilterableRepository<CustomTask, CustomTaskFilterBuilder>, ICustomTaskRepository
    {
        public CustomTaskRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<CustomTask>> GetFilteredItemsAsync(Action<CustomTaskFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<CustomTask> IncludeEntities(IQueryable<CustomTask> query)
        {
            if (_filterBuilder.IncludeSection)
            {
                query = query.Include(c => c.Section);
            }

            if (_filterBuilder.IncludeResponsibleUser)
            {
                query = query.Include(c => c.ResponsibleUser);
            }

            return query;
        }
    }
}
