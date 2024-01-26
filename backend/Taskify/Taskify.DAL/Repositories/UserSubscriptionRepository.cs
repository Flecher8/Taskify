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
    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {
        private readonly DataContext _dbContext;
        private readonly UserSubscriptionFilterBuilder _filterBuilder;

        public UserSubscriptionRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _filterBuilder = new UserSubscriptionFilterBuilder();
        }

        public async Task<UserSubscription> AddAsync(UserSubscription item)
        {
            await _dbContext.UserSubscriptions.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var userSubscription = await _dbContext.UserSubscriptions.FindAsync(id);
            if (userSubscription != null)
            {
                _dbContext.UserSubscriptions.Remove(userSubscription);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserSubscription>> GetAllAsync()
        {
            return await _dbContext.UserSubscriptions.ToListAsync();
        }

        public async Task<UserSubscription?> GetByIdAsync(string id)
        {
            return await _dbContext.UserSubscriptions.FindAsync(id);
        }

        public async Task<List<UserSubscription>> GetFilteredItemsAsync(Expression<Func<UserSubscription, bool>> filter)
        {
            return await _dbContext.UserSubscriptions.Where(filter).ToListAsync();
        }

        public async Task<List<UserSubscription>> GetFilteredItemsAsync(Action<UserSubscriptionFilterBuilder> buildFilter)
        {
            buildFilter(_filterBuilder);

            var query = _dbContext.UserSubscriptions.AsQueryable();

            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(us => us.User);
            }

            if (_filterBuilder.IncludeSubscription)
            {
                query = query.Include(us => us.Subscription);
            }

            return await query
                .Where(_filterBuilder.Filter)
                .ToListAsync();
        }

        public async Task UpdateAsync(UserSubscription item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
