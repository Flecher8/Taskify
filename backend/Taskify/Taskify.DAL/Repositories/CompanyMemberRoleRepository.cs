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
    public class CompanyMemberRoleRepository : ICompanyMemberRoleRepository
    {
        private readonly DataContext _dbContext;

        public CompanyMemberRoleRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CompanyMemberRole> AddAsync(CompanyMemberRole item)
        {
            await _dbContext.CompanyMemberRoles.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var companyMemberRole = await _dbContext.CompanyMemberRoles.FindAsync(id);
            if (companyMemberRole != null)
            {
                _dbContext.CompanyMemberRoles.Remove(companyMemberRole);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<CompanyMemberRole>> GetAllAsync()
        {
            return await _dbContext.CompanyMemberRoles.ToListAsync();
        }

        public async Task<CompanyMemberRole?> GetByIdAsync(string id)
        {
            return await _dbContext.CompanyMemberRoles.FindAsync(id);
        }

        public async Task<List<CompanyMemberRole>> GetFilteredItemsAsync(Expression<Func<CompanyMemberRole, bool>> filter)
        {
            return await _dbContext.CompanyMemberRoles.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(CompanyMemberRole item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
