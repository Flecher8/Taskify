using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class ProjectFilterBuilder : BaseFilterBuilder<Project>
    {
        public bool IncludeUser { get; private set; }
        public bool IncludeSections { get; private set; }
        public bool IncludeInvitations { get; private set; }
        public bool IncludeMembers { get; private set; }
        public bool IncludeRoles { get; private set; }
        public bool IncludeIncomes { get; private set; }

        public ProjectFilterBuilder IncludeUserEntity()
        {
            IncludeUser = true;
            return this;
        }

        public ProjectFilterBuilder IncludeSectionsEntity()
        {
            IncludeSections = true;
            return this;
        }

        public ProjectFilterBuilder IncludeInvitationsEntity()
        {
            IncludeInvitations = true;
            return this;
        }

        public ProjectFilterBuilder IncludeMembersEntity()
        {
            IncludeMembers = true;
            return this;
        }

        public ProjectFilterBuilder IncludeRolesEntity()
        {
            IncludeRoles = true;
            return this;
        }

        public ProjectFilterBuilder IncludeIncomesEntity()
        {
            IncludeIncomes = true;
            return this;
        }
    }
}
