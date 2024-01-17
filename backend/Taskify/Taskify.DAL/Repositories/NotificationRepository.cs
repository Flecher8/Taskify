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
    public class NotificationRepository : IDataRepository<Notification>
    {
        private readonly DataContext _dbContext;

        public NotificationRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Notification> AddAsync(Notification item)
        {
            await _dbContext.Notifications.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var notification = await _dbContext.Notifications.FindAsync(id);
            if (notification != null)
            {
                _dbContext.Notifications.Remove(notification);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _dbContext.Notifications.ToListAsync();
        }

        public async Task<Notification?> GetById(string id)
        {
            return await _dbContext.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetFilteredItemsAsync(Expression<Func<Notification, bool>> filter)
        {
            return await _dbContext.Notifications.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(Notification item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
