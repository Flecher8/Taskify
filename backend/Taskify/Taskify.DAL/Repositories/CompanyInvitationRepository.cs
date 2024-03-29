﻿using Microsoft.EntityFrameworkCore;
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
    public class CompanyInvitationRepository : ICompanyInvitationRepository
    {
        private readonly DataContext _dbContext;

        public CompanyInvitationRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CompanyInvitation> AddAsync(CompanyInvitation item)
        {
            await _dbContext.CompanyInvitations.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task DeleteAsync(string id)
        {
            var companyInvitation = await _dbContext.CompanyInvitations.FindAsync(id);
            if (companyInvitation != null)
            {
                _dbContext.CompanyInvitations.Remove(companyInvitation);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<CompanyInvitation>> GetAllAsync()
        {
            return await _dbContext.CompanyInvitations.ToListAsync();
        }

        public async Task<CompanyInvitation?> GetByIdAsync(string id)
        {
            return await _dbContext.CompanyInvitations.FindAsync(id);
        }

        public async Task<List<CompanyInvitation>> GetFilteredItemsAsync(Expression<Func<CompanyInvitation, bool>> filter)
        {
            return await _dbContext.CompanyInvitations.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(CompanyInvitation item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
