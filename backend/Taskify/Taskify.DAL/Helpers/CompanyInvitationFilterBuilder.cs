using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CompanyInvitationFilterBuilder
    {
        public bool IncludeNotification { get; private set; }
        public bool IncludeCompany { get; private set; }

        public Expression<Func<CompanyInvitation, bool>> Filter { get; private set; } = _ => true;

        public CompanyInvitationFilterBuilder IncludeNotificationEntity()
        {
            IncludeNotification = true;
            return this;
        }

        public CompanyInvitationFilterBuilder IncludeCompanyEntity()
        {
            IncludeCompany = true;
            return this;
        }

        public CompanyInvitationFilterBuilder WithFilter(Expression<Func<CompanyInvitation, bool>> filter)
        {
            Filter = filter;
            return this;
        }
    }
}
