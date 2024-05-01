using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;

namespace Taskify.DAL.Helpers
{
    public class TaskTimeTrackerFilterBuilder : BaseFilterBuilder<TaskTimeTracker>
    {
        public bool IncludeUser { get; private set; }
        public bool IncludeCustomTask { get; private set; }

        public TaskTimeTrackerFilterBuilder IncludeUserEntity()
        {
            IncludeUser = true;
            return this;
        }

        public TaskTimeTrackerFilterBuilder IncludeCustomTaskEntity()
        {
            IncludeCustomTask = true;
            return this;
        }
    }
}
