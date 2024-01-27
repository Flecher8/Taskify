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
    public class CompanyMemberRepository : BaseFilterableRepository<CompanyMember, CompanyMemberFilterBuilder>, ICompanyMemberRepository
    {
        public CompanyMemberRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<CompanyMember>> GetFilteredItemsAsync(Action<CompanyMemberFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<CompanyMember> IncludeEntities(IQueryable<CompanyMember> query)
        {
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

            return query;
        }
    }
}
