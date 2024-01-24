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
    public class ProjectMemberRepository : IProjectMemberRepository
    {
        private readonly DataContext _dbContext;

        public ProjectMemberRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectMember> AddAsync(ProjectMember item)
        {
            await _dbContext.ProjectMembers.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var projectMember = await _dbContext.ProjectMembers.FindAsync(id);
            if (projectMember != null)
            {
                _dbContext.ProjectMembers.Remove(projectMember);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ProjectMember>> GetAllAsync()
        {
            return await _dbContext.ProjectMembers.ToListAsync();
        }

        public async Task<ProjectMember?> GetById(string id)
        {
            return await _dbContext.ProjectMembers.FindAsync(id);
        }

        public async Task<List<ProjectMember>> GetFilteredItemsAsync(Expression<Func<ProjectMember, bool>> filter)
        {
            return await _dbContext.ProjectMembers.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(ProjectMember item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
