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
    public class NotificationRepository : BaseFilterableRepository<Notification, NotificationFilterBuilder>, INotificationRepository
    {
        public NotificationRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<Notification>> GetFilteredItemsAsync(Action<NotificationFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<Notification> IncludeEntities(IQueryable<Notification> query)
        {
            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(n => n.User);
            }

            return query;
        }
    }
}
