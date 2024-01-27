using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CompanyFilterBuilder : BaseFilterBuilder<Company>
    {
        public bool IncludeUser { get; private set; }
        public bool IncludeExpenses { get; private set; }
        public bool IncludeMembers { get; private set; }
        public bool IncludeMemberRoles { get; private set; }
        public bool IncludeInvitations { get; private set; }

        public CompanyFilterBuilder IncludeUserEntity()
        {
            IncludeUser = true;
            return this;
        }

        public CompanyFilterBuilder IncludeExpensesEntity()
        {
            IncludeExpenses = true;
            return this;
        }

        public CompanyFilterBuilder IncludeMembersEntity()
        {
            IncludeMembers = true;
            return this;
        }

        public CompanyFilterBuilder IncludeMemberRolesEntity()
        {
            IncludeMemberRoles = true;
            return this;
        }

        public CompanyFilterBuilder IncludeInvitationsEntity()
        {
            IncludeInvitations = true;
            return this;
        }
    }
}
