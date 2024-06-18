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
    public class ProjectInvitationRepository : BaseFilterableRepository<ProjectInvitation, ProjectInvitationFilterBuilder>, IProjectInvitationRepository
    {
        public ProjectInvitationRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<ProjectInvitation>> GetFilteredItemsAsync(Action<ProjectInvitationFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<ProjectInvitation> IncludeEntities(IQueryable<ProjectInvitation> query)
        {
            if (_filterBuilder.IncludeNotification)
            {
                query = query.Include(pi => pi.Notification);
            }

            if (_filterBuilder.IncludeProject)
            {
                query = query.Include(pi => pi.Project);
            }

            return query;
        }
    }
}
