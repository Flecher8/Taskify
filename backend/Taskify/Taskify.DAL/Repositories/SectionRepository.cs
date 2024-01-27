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
    public class SectionRepository : BaseFilterableRepository<Section, SectionFilterBuilder>, ISectionRepository
    {
        public SectionRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<Section>> GetFilteredItemsAsync(Action<SectionFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<Section> IncludeEntities(IQueryable<Section> query)
        {
            if (_filterBuilder.IncludeProject)
            {
                query = query.Include(s => s.Project);
            }

            if (_filterBuilder.IncludeCustomTasks)
            {
                query = query.Include(s => s.CustomTasks);
            }

            return query;
        }
    }
}
