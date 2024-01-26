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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _dbContext;
        private readonly CompanyFilterBuilder _filterBuilder;

        public CompanyRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _filterBuilder = new CompanyFilterBuilder();
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

        public async Task<Company?> GetByIdAsync(string id)
        {
            return await _dbContext.Companies.FindAsync(id);
        }

        public async Task<List<Company>> GetFilteredItemsAsync(Expression<Func<Company, bool>> filter)
        {
            return await _dbContext.Companies.Where(filter).ToListAsync();
        }

        public async Task<List<Company>> GetFilteredItemsAsync(Action<CompanyFilterBuilder> buildFilter)
        {
            buildFilter(_filterBuilder);

            var query = _dbContext.Companies.AsQueryable();

            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(c => c.User);
            }

            if (_filterBuilder.IncludeExpenses)
            {
                query = query.Include(c => c.CompanyExpenses);
            }

            if (_filterBuilder.IncludeMembers)
            {
                query = query.Include(c => c.CompanyMembers);
            }

            if (_filterBuilder.IncludeMemberRoles)
            {
                query = query.Include(c => c.CompanyMemberRoles);
            }

            if (_filterBuilder.IncludeInvitations)
            {
                query = query.Include(c => c.CompanyInvitations);
            }

            return await query
                .Where(_filterBuilder.Filter)
                .ToListAsync();
        }

        public async Task UpdateAsync(Company item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
