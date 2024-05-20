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
    public class CompanyRoleRepository : BaseFilterableRepository<CompanyRole, CompanyRoleFilterBuilder>, ICompanyRoleRepository
    {
        public CompanyRoleRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<CompanyRole>> GetFilteredItemsAsync(Action<CompanyRoleFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<CompanyRole> IncludeEntities(IQueryable<CompanyRole> query)
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
