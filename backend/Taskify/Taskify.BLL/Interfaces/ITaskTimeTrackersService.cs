using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ITaskTimeTrackersService
    {
        Task<Result<TaskTimeTracker>> CreateTaskTimeTrackerAsync(TaskTimeTracker taskTimeTracker);
        Task<Result<bool>> UpdateTaskTimeTrackerAsync(TaskTimeTracker taskTimeTracker);
        Task<Result<bool>> DeleteTaskTimeTrackerAsync(string id);
        Task<Result<List<TaskTimeTracker>>> GetTaskTimeTrackerByUserForTaskAsync(string userId, string taskId);
    }
}
