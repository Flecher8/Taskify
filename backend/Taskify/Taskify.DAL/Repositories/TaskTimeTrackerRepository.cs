using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.DAL.Helpers;
using Taskify.DAL.Interfaces;

namespace Taskify.DAL.Repositories
{
    public class TaskTimeTrackerRepository : BaseFilterableRepository<TaskTimeTracker, TaskTimeTrackerFilterBuilder>, ITaskTimeTrackerRepository
    {
        public TaskTimeTrackerRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<TaskTimeTracker>> GetFilteredItemsAsync(Action<TaskTimeTrackerFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<TaskTimeTracker> IncludeEntities(IQueryable<TaskTimeTracker> query)
        {
            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(us => us.User);
            }

            if (_filterBuilder.IncludeCustomTask)
            {
                query = query.Include(us => us.CustomTask);
            }

            return query;
        }
    }
}
