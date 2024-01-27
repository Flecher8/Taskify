using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CompanyMemberRoleFilterBuilder : BaseFilterBuilder<CompanyMemberRole>
    {
        public bool IncludeCompany { get; private set; }
        public bool IncludeMembers { get; private set; }

        public CompanyMemberRoleFilterBuilder IncludeCompanyEntity()
        {
            IncludeCompany = true;
            return this;
        }

        public CompanyMemberRoleFilterBuilder IncludeMembersEntity()
        {
            IncludeMembers = true;
            return this;
        }
    }
}
