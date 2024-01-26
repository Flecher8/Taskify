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
    public class CompanyMemberRepository : ICompanyMemberRepository
    {
        private readonly DataContext _dbContext;

        public CompanyMemberRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task UpdateAsync(CompanyMember item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
