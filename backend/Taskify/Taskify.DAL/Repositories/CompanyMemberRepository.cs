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
    public class CompanyMemberRepository : ICompanyMemberRepository
    {
        private readonly DataContext _dbContext;
        private readonly CompanyMemberFilterBuilder _filterBuilder;

        public CompanyMemberRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _filterBuilder = new CompanyMemberFilterBuilder();
        }

        public async Task<CompanyMember> AddAsync(CompanyMember item)
        {
            await _dbContext.CompanyMembers.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var companyMember = await _dbContext.CompanyMembers.FindAsync(id);
            if (companyMember != null)
            {
                _dbContext.CompanyMembers.Remove(companyMember);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<CompanyMember>> GetAllAsync()
        {
            return await _dbContext.CompanyMembers.ToListAsync();
        }

        public async Task<CompanyMember?> GetByIdAsync(string id)
        {
            return await _dbContext.CompanyMembers.FindAsync(id);
        }

        public async Task<List<CompanyMember>> GetFilteredItemsAsync(Expression<Func<CompanyMember, bool>> filter)
        {
            return await _dbContext.CompanyMembers.Where(filter).ToListAsync();
        }

        public async Task<List<CompanyMember>> GetFilteredItemsAsync(Action<CompanyMemberFilterBuilder> buildFilter)
        {
            buildFilter(_filterBuilder);

            var query = _dbContext.CompanyMembers.AsQueryable();

            if (_filterBuilder.IncludeUser)
            {
                query = query.Include(cm => cm.User);
            }

            if (_filterBuilder.IncludeCompany)
            {
                query = query.Include(cm => cm.Company);
            }

            if (_filterBuilder.IncludeRole)
            {
                query = query.Include(cm => cm.Role);
            }

            return await query
                .Where(_filterBuilder.Filter)
                .ToListAsync();
        }

        public async Task UpdateAsync(CompanyMember item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
