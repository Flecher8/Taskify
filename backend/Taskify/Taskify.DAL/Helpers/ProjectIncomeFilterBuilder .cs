using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class ProjectIncomeFilterBuilder : BaseFilterBuilder<ProjectIncome>
    {
        public bool IncludeProject { get; private set; }

        public ProjectIncomeFilterBuilder IncludeProjectEntity()
        {
            IncludeProject = true;
            return this;
        }
    }
}
