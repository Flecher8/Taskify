using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class ProjectRoleFilterBuilder : BaseFilterBuilder<ProjectRole>
    {
        public bool IncludeProject { get; private set; }

        public ProjectRoleFilterBuilder IncludeProjectEntity()
        {
            IncludeProject = true;
            return this;
        }
    }
}
