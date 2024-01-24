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
    public class ProjectIncomeRepository : IProjectIncomeRepository
    {
        private readonly DataContext _dbContext;

        public ProjectIncomeRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProjectIncome> AddAsync(ProjectIncome item)
        {
            await _dbContext.ProjectIncomes.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var projectIncome = await _dbContext.ProjectIncomes.FindAsync(id);
            if (projectIncome != null)
            {
                _dbContext.ProjectIncomes.Remove(projectIncome);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ProjectIncome>> GetAllAsync()
        {
            return await _dbContext.ProjectIncomes.ToListAsync();
        }

        public async Task<ProjectIncome?> GetById(string id)
        {
            return await _dbContext.ProjectIncomes.FindAsync(id);
        }

        public async Task<List<ProjectIncome>> GetFilteredItemsAsync(Expression<Func<ProjectIncome, bool>> filter)
        {
            return await _dbContext.ProjectIncomes.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(ProjectIncome item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
