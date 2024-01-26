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
    public class CompanyExpenseRepository : ICompanyExpenseRepository
    {
        private readonly DataContext _dbContext;
        private readonly CompanyExpenseFilterBuilder _filterBuilder;

        public CompanyExpenseRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _filterBuilder = new CompanyExpenseFilterBuilder();
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

        public async Task<List<CompanyExpense>> GetAllAsync()
        {
            return await _dbContext.CompanyExpenses.ToListAsync();
        }

        public async Task<CompanyExpense?> GetByIdAsync(string id)
        {
            return await _dbContext.CompanyExpenses.FindAsync(id);
        }

        public async Task<List<CompanyExpense>> GetFilteredItemsAsync(Expression<Func<CompanyExpense, bool>> filter)
        {
            return await _dbContext.CompanyExpenses.Where(filter).ToListAsync();
        }

        public async Task<List<CompanyExpense>> GetFilteredItemsAsync(Action<CompanyExpenseFilterBuilder> buildFilter)
        {
            buildFilter(_filterBuilder);

            var query = _dbContext.CompanyExpenses.AsQueryable();

            if (_filterBuilder.IncludeCompany)
            {
                query = query.Include(ce => ce.Company);
            }

            return await query
                .Where(_filterBuilder.Filter)
                .ToListAsync();
        }

        public async Task UpdateAsync(CompanyExpense item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
