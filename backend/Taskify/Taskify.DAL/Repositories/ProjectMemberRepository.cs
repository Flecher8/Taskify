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
    public class ProjectMemberRepository : BaseFilterableRepository<ProjectMember, ProjectMemberFilterBuilder>, IProjectMemberRepository
    {
        public ProjectMemberRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<ProjectMember>> GetFilteredItemsAsync(Action<ProjectMemberFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<ProjectMember> IncludeEntities(IQueryable<ProjectMember> query)
        {
            if (_filterBuilder.IncludeProject)
            {
                query = query.Include(pm => pm.Project);
            }

            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(pm => pm.User);
            }

            if (_filterBuilder.IncludeProjectRole)
            {
                query = query.Include(pm => pm.ProjectRole);
            }

            return query;
        }
    }
}
