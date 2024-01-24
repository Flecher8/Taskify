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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _dbContext;

        public CompanyRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Company> AddAsync(Company item)
        {
            await _dbContext.Companies.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var company = await _dbContext.Companies.FindAsync(id);
            if (company != null)
            {
                _dbContext.Companies.Remove(company);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Company>> GetAllAsync()
        {
            return await _dbContext.Companies.ToListAsync();
        }

        public async Task<Company?> GetById(string id)
        {
            return await _dbContext.Companies.FindAsync(id);
        }

        public async Task<List<Company>> GetFilteredItemsAsync(Expression<Func<Company, bool>> filter)
        {
            return await _dbContext.Companies.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(Company item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
