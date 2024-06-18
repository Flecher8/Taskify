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
    public class CompanyInvitationRepository : BaseFilterableRepository<CompanyInvitation, CompanyInvitationFilterBuilder>, ICompanyInvitationRepository
    {
        public CompanyInvitationRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<CompanyInvitation>> GetFilteredItemsAsync(Action<CompanyInvitationFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<CompanyInvitation> IncludeEntities(IQueryable<CompanyInvitation> query)
        {
            if (_filterBuilder.IncludeNotification)
            {
                query = query.Include(ci => ci.Notification);
            }

            if (_filterBuilder.IncludeCompany)
            {
                query = query.Include(ci => ci.Company);
            }

            return query;
        }
    }
}
