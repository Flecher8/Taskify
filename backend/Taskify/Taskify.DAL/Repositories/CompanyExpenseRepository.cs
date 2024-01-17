using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Repositories
{
    public class CompanyExpenseRepository
    {
        private readonly DataContext _dbContext;

        public CompanyExpenseRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CompanyExpense> AddAsync(CompanyExpense item)
        {
            await _dbContext.CompanyExpenses.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var companyExpense = await _dbContext.CompanyExpenses.FindAsync(id);
            if (companyExpense != null)
            {
                _dbContext.CompanyExpenses.Remove(companyExpense);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CompanyExpense>> GetAllAsync()
        {
            return await _dbContext.CompanyExpenses.ToListAsync();
        }

        public async Task<CompanyExpense?> GetById(string id)
        {
            return await _dbContext.CompanyExpenses.FindAsync(id);
        }

        public async Task<IEnumerable<CompanyExpense>> GetFilteredItemsAsync(Expression<Func<CompanyExpense, bool>> filter)
        {
            return await _dbContext.CompanyExpenses.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(CompanyExpense item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
