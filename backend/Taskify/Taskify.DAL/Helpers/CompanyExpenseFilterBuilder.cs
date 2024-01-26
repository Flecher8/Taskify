using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CompanyExpenseFilterBuilder
    {
        public bool IncludeCompany { get; private set; }

        public Expression<Func<CompanyExpense, bool>> Filter { get; private set; } = _ => true;

        public CompanyExpenseFilterBuilder IncludeCompanyEntity()
        {
            IncludeCompany = true;
            return this;
        }

        public CompanyExpenseFilterBuilder WithFilter(Expression<Func<CompanyExpense, bool>> filter)
        {
            Filter = filter;
            return this;
        }
    }
}
