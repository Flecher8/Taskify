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
    public class CompanyMemberRoleRepository : BaseFilterableRepository<CompanyMemberRole, CompanyMemberRoleFilterBuilder>, ICompanyMemberRoleRepository
    {
        public CompanyMemberRoleRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<CompanyMemberRole>> GetFilteredItemsAsync(Action<CompanyMemberRoleFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<CompanyMemberRole> IncludeEntities(IQueryable<CompanyMemberRole> query)
        {
            if (_filterBuilder.IncludeCompany)
            {
                query = query.Include(cmr => cmr.Company);
            }

            if (_filterBuilder.IncludeMembers)
            {
                query = query.Include(cmr => cmr.CompanyMembers);
            }

            return query;
        }
    }
}
