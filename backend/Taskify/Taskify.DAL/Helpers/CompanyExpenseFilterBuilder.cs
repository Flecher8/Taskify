using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CompanyExpenseFilterBuilder : BaseFilterBuilder<CompanyExpense>
    {
        public bool IncludeCompany { get; private set; }

        public CompanyExpenseFilterBuilder IncludeCompanyEntity()
        {
            IncludeCompany = true;
            return this;
        }
    }
}
