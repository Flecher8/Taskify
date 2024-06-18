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
    public class ProjectRoleRepository : BaseFilterableRepository<ProjectRole, ProjectRoleFilterBuilder>, IProjectRoleRepository
    {
        public ProjectRoleRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<ProjectRole>> GetFilteredItemsAsync(Action<ProjectRoleFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<ProjectRole> IncludeEntities(IQueryable<ProjectRole> query)
        {
            if (_filterBuilder.IncludeProject)
            {
                query = query.Include(pr => pr.Project);
            }

            return query;
        }
    }
}
