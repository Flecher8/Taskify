using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class CompanyInvitationFilterBuilder : BaseFilterBuilder<CompanyInvitation>
    {
        public bool IncludeNotification { get; private set; }
        public bool IncludeCompany { get; private set; }

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
    }
}
