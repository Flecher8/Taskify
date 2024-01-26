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
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _dbContext;
        private readonly ProjectFilterBuilder _filterBuilder;

        public ProjectRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _filterBuilder = new ProjectFilterBuilder();
        }

        public async Task<Project> AddAsync(Project item)
        {
            await _dbContext.Projects.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var project = await _dbContext.Projects.FindAsync(id);
            if (project != null)
            {
                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _dbContext.Projects.ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(string id)
        {
            return await _dbContext.Projects.FindAsync(id);
        }

        public async Task<List<Project>> GetFilteredItemsAsync(Expression<Func<Project, bool>> filter)
        {
            return await _dbContext.Projects.Where(filter).ToListAsync();
        }

        public async Task<List<Project>> GetFilteredItemsAsync(Action<ProjectFilterBuilder> buildFilter)
        {
            buildFilter(_filterBuilder);

            var query = _dbContext.Projects.AsQueryable();

            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(p => p.User);
            }

            if (_filterBuilder.IncludeSections)
            {
                query = query.Include(p => p.Sections);
            }

            if (_filterBuilder.IncludeInvitations)
            {
                query = query.Include(p => p.ProjectInvitations);
            }

            if (_filterBuilder.IncludeMembers)
            {
                query = query.Include(p => p.ProjectMembers);
            }

            if (_filterBuilder.IncludeRoles)
            {
                query = query.Include(p => p.ProjectRoles);
            }

            return await query
                .Where(_filterBuilder.Filter)
                .ToListAsync();
        }

        public async Task UpdateAsync(Project item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
