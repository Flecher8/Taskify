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
    public class ProjectRepository : BaseFilterableRepository<Project, ProjectFilterBuilder>, IProjectRepository
    {
        public ProjectRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<Project>> GetFilteredItemsAsync(Action<ProjectFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<Project> IncludeEntities(IQueryable<Project> query)
        {
            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(p => p.User);
            }

            if (_filterBuilder.IncludeSections)
            {
                query = query.Include(p => p.Sections);
            }

            if (_filterBuilder.IncludeInvitations)
            {
                query = query.Include(p => p.ProjectInvitations);
            }

            if (_filterBuilder.IncludeMembers)
            {
                query = query.Include(p => p.ProjectMembers);
            }

            if (_filterBuilder.IncludeRoles)
            {
                query = query.Include(p => p.ProjectRoles);
            }

            return query;
        }
    }
}
