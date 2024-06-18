using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CompanyRoleFilterBuilder : BaseFilterBuilder<CompanyRole>
    {
        public bool IncludeCompany { get; private set; }
        public bool IncludeMembers { get; private set; }

        public CompanyRoleFilterBuilder IncludeCompanyEntity()
        {
            IncludeCompany = true;
            return this;
        }

        public CompanyRoleFilterBuilder IncludeMembersEntity()
        {
            IncludeMembers = true;
            return this;
        }
    }
}
