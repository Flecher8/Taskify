using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class SectionFilterBuilder : BaseFilterBuilder<Section>
    {
        public bool IncludeProject { get; private set; }
        public bool IncludeCustomTasks { get; private set; }

        public SectionFilterBuilder IncludeProjectEntity()
        {
            IncludeProject = true;
            return this;
        }

        public SectionFilterBuilder IncludeCustomTasksEntity()
        {
            IncludeCustomTasks = true;
            return this;
        }
    }
}
