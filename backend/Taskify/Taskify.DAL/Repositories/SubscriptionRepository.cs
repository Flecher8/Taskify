using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.DAL.Interfaces;

namespace Taskify.DAL.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DataContext _dbContext;

        public SubscriptionRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Subscription> AddAsync(Subscription item)
        {
            await _dbContext.Subscriptions.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var subscription = await _dbContext.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _dbContext.Subscriptions.Remove(subscription);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Subscription>> GetAllAsync()
        {
            return await _dbContext.Subscriptions.ToListAsync();
        }

        public async Task<Subscription?> GetByIdAsync(string id)
        {
            return await _dbContext.Subscriptions.FindAsync(id);
        }

        public async Task<List<Subscription>> GetFilteredItemsAsync(Expression<Func<Subscription, bool>> filter)
        {
            return await _dbContext.Subscriptions.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(Subscription item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
