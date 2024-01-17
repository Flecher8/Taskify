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
    public class CustomTaskRepository : IDataRepository<CustomTask>
    {
        private readonly DataContext _dbContext;

        public CustomTaskRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomTask> AddAsync(CustomTask item)
        {
            await _dbContext.CustomTasks.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var customTask = await _dbContext.CustomTasks.FindAsync(id);
            if (customTask != null)
            {
                _dbContext.CustomTasks.Remove(customTask);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CustomTask>> GetAllAsync()
        {
            return await _dbContext.CustomTasks.ToListAsync();
        }

        public async Task<CustomTask?> GetById(string id)
        {
            return await _dbContext.CustomTasks.FindAsync(id);
        }

        public async Task<IEnumerable<CustomTask>> GetFilteredItemsAsync(Expression<Func<CustomTask, bool>> filter)
        {
            return await _dbContext.CustomTasks.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(CustomTask item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
