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
    public class UserSubscriptionRepository : BaseFilterableRepository<UserSubscription, UserSubscriptionFilterBuilder>, IUserSubscriptionRepository
    {
        public UserSubscriptionRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<UserSubscription>> GetFilteredItemsAsync(Func<IQueryable<UserSubscription>, IQueryable<UserSubscription>> includeEntities, Action<UserSubscriptionFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(IncludeEntities, buildFilter);
        }

        private IQueryable<UserSubscription> IncludeEntities(IQueryable<UserSubscription> query)
        {
            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(us => us.User);
            }

            if (_filterBuilder.IncludeSubscription)
            {
                query = query.Include(us => us.Subscription);
            }

            return query;
        }
    }
}
