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
    public class CompanyExpenseRepository : BaseFilterableRepository<CompanyExpense, CompanyExpenseFilterBuilder>, ICompanyExpenseRepository
    {

        public CompanyExpenseRepository(DataContext dbContext) : base(dbContext) { }

        public override async Task<List<CompanyExpense>> GetFilteredItemsAsync(Action<CompanyExpenseFilterBuilder> buildFilter)
        {
            return await base.GetFilteredItemsAsync(buildFilter);
        }

        protected override IQueryable<CompanyExpense> IncludeEntities(IQueryable<CompanyExpense> query)
        {
            if (_filterBuilder.IncludeCompany)
            {
                query = query.Include(ce => ce.Company);
            }

            return query;
        }
    }
}
