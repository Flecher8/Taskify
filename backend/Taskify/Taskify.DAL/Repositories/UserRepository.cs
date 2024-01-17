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
    public class UserRepository : IDataRepository<User>
    {
        private readonly DataContext _dbContext;

        public UserRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> AddAsync(User item)
        {
            await _dbContext.Users.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetById(string id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetFilteredItemsAsync(Expression<Func<User, bool>> filter)
        {
            return await _dbContext.Users.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(User item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
