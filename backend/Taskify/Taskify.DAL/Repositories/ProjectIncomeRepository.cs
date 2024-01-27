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
    public class ProjectIncomeRepository : BaseFilterableRepository<ProjectIncome, ProjectIncomeFilterBuilder>, IProjectIncomeRepository
    {
        public ProjectIncomeRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<ProjectIncome>> GetFilteredItemsAsync(Action<ProjectIncomeFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<ProjectIncome> IncludeEntities(IQueryable<ProjectIncome> query)
        {
            if (_filterBuilder.IncludeProject)
            {
                query = query.Include(pi => pi.Project);
            }

            return query;
        }
    }
}
