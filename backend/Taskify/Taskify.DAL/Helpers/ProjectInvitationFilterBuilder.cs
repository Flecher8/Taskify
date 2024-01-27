using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class ProjectInvitationFilterBuilder : BaseFilterBuilder<ProjectInvitation>
    {
        public bool IncludeNotification { get; private set; }
        public bool IncludeProject { get; private set; }

        public ProjectInvitationFilterBuilder IncludeNotificationEntity()
        {
            IncludeNotification = true;
            return this;
        }

        public ProjectInvitationFilterBuilder IncludeProjectEntity()
        {
            IncludeProject = true;
            return this;
        }
    }
}
