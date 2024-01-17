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
    public class ProjectInvitationRepository : IDataRepository<ProjectInvitation>
    {
        private readonly DataContext _dbContext;

        public ProjectInvitationRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectInvitation> AddAsync(ProjectInvitation item)
        {
            await _dbContext.ProjectInvitations.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var projectInvitation = await _dbContext.ProjectInvitations.FindAsync(id);
            if (projectInvitation != null)
            {
                _dbContext.ProjectInvitations.Remove(projectInvitation);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProjectInvitation>> GetAllAsync()
        {
            return await _dbContext.ProjectInvitations.ToListAsync();
        }

        public async Task<ProjectInvitation?> GetById(string id)
        {
            return await _dbContext.ProjectInvitations.FindAsync(id);
        }

        public async Task<IEnumerable<ProjectInvitation>> GetFilteredItemsAsync(Expression<Func<ProjectInvitation, bool>> filter)
        {
            return await _dbContext.ProjectInvitations.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(ProjectInvitation item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
