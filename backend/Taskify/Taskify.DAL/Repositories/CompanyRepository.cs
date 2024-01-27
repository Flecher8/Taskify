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
    public class CompanyRepository : BaseFilterableRepository<Company, CompanyFilterBuilder>, ICompanyRepository
    {
        public CompanyRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<Company>> GetFilteredItemsAsync(Action<CompanyFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<Company> IncludeEntities(IQueryable<Company> query)
        {
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

            return query;
        }
    }
}
