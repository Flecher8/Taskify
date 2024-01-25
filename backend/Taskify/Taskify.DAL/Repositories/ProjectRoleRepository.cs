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
    public class ProjectRoleRepository : IProjectRoleRepository
    {
        private readonly DataContext _dbContext;

        public ProjectRoleRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectRole> AddAsync(ProjectRole item)
        {
            await _dbContext.ProjectRoles.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var projectRole = await _dbContext.ProjectRoles.FindAsync(id);
            if (projectRole != null)
            {
                _dbContext.ProjectRoles.Remove(projectRole);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ProjectRole>> GetAllAsync()
        {
            return await _dbContext.ProjectRoles.ToListAsync();
        }

        public async Task<ProjectRole?> GetByIdAsync(string id)
        {
            return await _dbContext.ProjectRoles.FindAsync(id);
        }

        public async Task<List<ProjectRole>> GetFilteredItemsAsync(Expression<Func<ProjectRole, bool>> filter)
        {
            return await _dbContext.ProjectRoles.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(ProjectRole item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
