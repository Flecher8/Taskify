using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class ProjectMemberFilterBuilder : BaseFilterBuilder<ProjectMember>
    {
        public bool IncludeProject { get; private set; }
        public bool IncludeUser { get; private set; }
        public bool IncludeProjectRole { get; private set; }

        public ProjectMemberFilterBuilder IncludeProjectEntity()
        {
            IncludeProject = true;
            return this;
        }

        public ProjectMemberFilterBuilder IncludeUserEntity()
        {
            IncludeUser = true;
            return this;
        }

        public ProjectMemberFilterBuilder IncludeProjectRoleEntity()
        {
            IncludeProjectRole = true;
            return this;
        }
    }
}
